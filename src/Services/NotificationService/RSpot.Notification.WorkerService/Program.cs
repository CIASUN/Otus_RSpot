using RSpot.Notification.Infrastructure.Messaging;
using Serilog;
using Serilog.Events;

namespace RSpot.Notification.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Настройка Serilog ДО создания builder
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // менее подробный лог от Microsoft
                .MinimumLevel.Information()                                // по умолчанию логировать начиная с Info
                .Enrich.FromLogContext()                                   // добавляет полезные поля (например, RequestId)
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties}{NewLine}{Exception}")
                .WriteTo.File("Logs/notification-log.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj} {Properties}{NewLine}{Exception}")
                .CreateLogger();

            try
            {
                Log.Information("Starting Notification Worker Service");

                var builder = Host.CreateApplicationBuilder(args);

                builder.Services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddSerilog();
                });

                builder.Services.AddHostedService<BookingCreatedConsumer>();

                var host = builder.Build();
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Notification Worker Service terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
