using RSpot.Booking.Domain.Models;

namespace RSpot.Booking.Application.Interfaces;

public interface IBookingRepository
{
    Task<IEnumerable<RSpot.Booking.Domain.Models.Booking>> GetByUserIdAsync(string userId);
    Task AddAsync(RSpot.Booking.Domain.Models.Booking booking);
}
