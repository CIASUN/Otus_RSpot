using RSpot.Booking.Application.DTOs;

namespace RSpot.Booking.Application.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetBookingsForUserAsync(string userId);
        Task CreateBookingAsync(string userId, CreateBookingRequest request);
    }
}