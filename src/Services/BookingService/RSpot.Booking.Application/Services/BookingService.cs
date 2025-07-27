using RSpot.Booking.Application.Interfaces;
using RSpot.Booking.Application.DTOs;

namespace RSpot.Booking.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<BookingDto>> GetBookingsForUserAsync(string userId)
        {
            var bookings = await _bookingRepository.GetByUserIdAsync(userId);
            return bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                WorkspaceId = b.WorkspaceId,
                StartTime = b.StartTime,
                EndTime = b.EndTime
            });
        }
    }
}