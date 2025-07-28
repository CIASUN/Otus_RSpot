using RSpot.Booking.Application.DTOs;

namespace RSpot.Booking.Application.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetBookingsForUserAsync(string userId);
        Task<BookingDto> CreateBookingAsync(string userId, CreateBookingRequest request);
    }
}