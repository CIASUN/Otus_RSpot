
namespace RSpot.Booking.Application.DTOs
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public Guid Userid { get; set; }
        public Guid WorkspaceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public WorkspaceDto? Workspace { get; set; }
    }
}