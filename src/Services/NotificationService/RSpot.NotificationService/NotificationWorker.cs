namespace RSpot.NotificationService
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public class NotificationWorker : BackgroundService
    {
        private readonly ILogger<NotificationWorker> _logger;

        public NotificationWorker(ILogger<NotificationWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NotificationWorker started.");

            // Здесь логика подключения к RabbitMQ и обработки сообщений
            while (!stoppingToken.IsCancellationRequested)
            {
                // Подключиться и слушать очередь RabbitMQ
                // Обработать сообщения
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation("NotificationWorker stopped.");
        }
    }

}
