namespace RSpot.Users.API
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

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
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

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // ** Инициализация отключена **
            /*
            if (runDbInit)
            {
                using var scope = app.Services.CreateScope();
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<UsersDbContext>();
                var passwordHasher = services.GetRequiredService<IPasswordHasher<User>>();
                var userRepo = services.GetRequiredService<IUserRepository>();

                DbInitializer.Initialize(context, passwordHasher, userRepo);
                Console.WriteLine("Создание администратора завершено");
            }
            */

            app.Run();
        }
    }
}
