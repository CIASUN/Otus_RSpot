using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace RSpot.Notification.Infrastructure.Messaging;

public class BookingCreatedConsumer : BackgroundService
{
    private readonly ILogger<BookingCreatedConsumer> _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public BookingCreatedConsumer(ILogger<BookingCreatedConsumer> logger)
    {
        _logger = logger;

        var factory = new ConnectionFactory() { HostName = "rabbitmq-rspot" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: "booking-created",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("NotificationService слушает очередь booking-created...");

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                Console.WriteLine("===> RAW JSON: " + json);
                var booking = JsonSerializer.Deserialize<BookingCreatedMessage>(json);

                if (booking != null)
                {
                    _logger.LogInformation(
                        "Получено событие: BookingId={BookingId}, UserId={UserId}, WorkspaceId={WorkspaceId}, Start={Start}, End={End}",
                        booking.BookingId,
                        booking.UserId,
                        booking.WorkspaceId,
                        booking.StartTime,
                        booking.EndTime
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке события BookingCreated");
            }
        };

        _channel.BasicConsume(
            queue: "booking-created",
            autoAck: true,
            consumer: consumer
        );

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
