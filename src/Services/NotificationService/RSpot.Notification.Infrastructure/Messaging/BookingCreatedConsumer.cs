using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace RSpot.Notification.Infrastructure.Messaging
{
    public class BookingCreatedConsumer : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq-rspot" }; // имя контейнера в docker-compose
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "booking-created",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                try
                {
                    var booking = JsonSerializer.Deserialize<BookingCreatedMessage>(json);

                    Console.WriteLine($"[NotificationService] Новое бронирование: {booking.BookingId}, пользователь: {booking.UserId}");

                    // TODO: отправить email, пуш, или записать в базу
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[NotificationService] Ошибка обработки события: {ex.Message}");
                }
            };

            _channel.BasicConsume(queue: "booking-created",
                                 autoAck: true,
                                 consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }

    public class BookingCreatedMessage
    {
        public string BookingId { get; set; }
        public string UserId { get; set; }
        public string WorkspaceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
