using RabbitMQ.Client;
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

                // ПОДКЛЮЧЕНИЕ К RabbitMQ С RETRY  on_depend в docker-compose есть, он дает гарантию что контейнер есть, но БД в нем еще не доступна, и нужно как далее сделано, без отого не работало.
                var maxRetries = 10;
                var delaySeconds = 5;
                var factory = new ConnectionFactory() { HostName = "rabbitmq-rspot" };

                for (int attempt = 1; attempt <= maxRetries; attempt++)
                {
                    try
                    {
                        using var connection = factory.CreateConnection();
                        using var channel = connection.CreateModel();
                        channel.QueueDeclare(queue: "booking-created", durable: false, exclusive: false, autoDelete: false, arguments: null);
                        Console.WriteLine($"[Startup] Successfully connected to RabbitMQ on attempt {attempt}");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Startup] Failed to connect to RabbitMQ (attempt {attempt}/{maxRetries}): {ex.Message}");
                        if (attempt == maxRetries) throw;
                        Thread.Sleep(delaySeconds * 1000);
                    }
                }


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
