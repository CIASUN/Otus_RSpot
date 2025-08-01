﻿namespace RSpot.Users.API
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using RSpot.Users.Application.Services.Interfaces;
    using RSpot.Users.Application.Services;
    using RSpot.Users.Application.Configuration;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using RSpot.Users.Domain.Models;
    using RSpot.Users.Domain.Interfaces;
    using RSpot.Users.Infrastructure.Auth;
    using RSpot.Users.Infrastructure.Persistence;
    using RSpot.Users.Infrastructure.Seed;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var frontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? "http://localhost:5173";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.WithOrigins(frontendUrl)
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            // Отключаем инициализацию из кода, т.к. база создаётся SQL-скриптом в контейнере Postgres
            var runDbInit = false; // builder.Configuration.GetValue<bool>("RunDbInit", true);

            builder.WebHost.UseUrls("http://0.0.0.0:80");

            // 1. Настройки JWT из appsettings или переменных окружения
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
            Console.WriteLine($"[Config] JWT Secret starts with: {jwtSettings.Secret?.Substring(0, 5) ?? "<null>"}");
            Console.WriteLine($"[Config] UsersDb = {builder.Configuration.GetConnectionString("UsersDb")}");

            var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

            // 2. Подключаем сервис генерации токенов
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            // 3. Настраиваем JWT аутентификацию
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true
                };
            });

            builder.Services.AddAuthorization();

            // 4. DI: EF Core + User сервисы и репозитории
            builder.Services.AddDbContext<UsersDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("UsersDb")));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            // ПОДКЛЮЧЕНИЕ К БАЗЕ С RETRY  on_depend в docker-compose есть, он дает гарантию что контейнер есть, но БД в нем еще не доступна, и нужно как далее сделано, без отого не работало.
            var maxRetries = 10;
            var delaySeconds = 5;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    using var scope = builder.Services.BuildServiceProvider().CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
                    db.Database.EnsureCreated(); // или db.Database.CanConnect() / Migrate()
                    Console.WriteLine($"[Startup] Successfully connected to UsersDb on attempt {attempt}");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Startup] Failed to connect to UsersDb (attempt {attempt}/{maxRetries}): {ex.Message}");
                    if (attempt == maxRetries) throw; // бросаем исключение, если не смогли после всех попыток
                    Thread.Sleep(delaySeconds * 1000);
                }
            }

            // Swagger + API
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "RSpot.Users.API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Введите JWT токен в формате: Bearer {токен}"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            var app = builder.Build();

            // Вызов сидера админа
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
                await DbSeeder.SeedAdminAsync(db);
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // app.UseHttpsRedirection(); //  не используе https

            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
