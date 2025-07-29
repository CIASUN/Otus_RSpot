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
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Information()                               
                .Enrich.FromLogContext()                                   
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

                builder.Logging.ClearProviders();
                builder.Logging.AddSerilog(Log.Logger);
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
