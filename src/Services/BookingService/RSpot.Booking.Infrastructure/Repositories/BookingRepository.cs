using Microsoft.EntityFrameworkCore;
using RSpot.Booking.Application.Interfaces;
using RSpot.Booking.Domain.Models;
using RSpot.Booking.Infrastructure.Persistence;

namespace RSpot.Booking.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly BookingDbContext _context;

    public BookingRepository(BookingDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RSpot.Booking.Domain.Models.Booking>> GetByUserIdAsync(string userId)
    {

        if (!Guid.TryParse(userId, out var userGuid))
            throw new ArgumentException("Invalid userId format", nameof(userId));

        return await _context.Bookings
            .Where(b => b.UserId == userGuid)
            .OrderByDescending(b => b.StartTime)
            .ToListAsync();
    }

    public async Task AddAsync(RSpot.Booking.Domain.Models.Booking booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
    }

}
