using RSpot.Booking.Domain.Models;
public class BookingServiceTests
{
    [Fact]
    public void CanCreateBooking()
    {
        // Arrange
        var booking = new Booking { Id = Guid.NewGuid(), WorkspaceId = Guid.NewGuid(), UserId = Guid.NewGuid() };

        // Act
        // ������ �������� ���� ��� ������

        // Assert
        Assert.NotNull(booking);
    }
}
