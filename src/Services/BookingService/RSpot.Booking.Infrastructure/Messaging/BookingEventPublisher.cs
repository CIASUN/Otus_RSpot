using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using RSpot.Booking.Application.Interfaces;

namespace RSpot.Booking.Infrastructure.Messaging;

public class BookingEventPublisher : IBookingEventPublisher
{
    private readonly IConnection _connection;
    private readonly RabbitMQ.Client.IModel _channel;

    public BookingEventPublisher()
    {
        var factory = new ConnectionFactory { HostName = "rabbitmq-rspot" }; // или localhost
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "booking-created",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    public Task PublishBookingCreatedAsync(string bookingId, string userId, Guid workspaceId, DateTime startTime, DateTime endTime)
    {
        var message = new
        {
            BookingId = bookingId,
            UserId = userId,
            WorkspaceId = workspaceId,
            StartTime = startTime,
            EndTime = endTime
        };

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        _channel.BasicPublish(exchange: "",
                             routingKey: "booking-created",
                             basicProperties: null,
                             body: body);

        return Task.CompletedTask;
    }
}
