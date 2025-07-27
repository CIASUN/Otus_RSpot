using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSpot.Booking.Application.Interfaces;
using RSpot.Booking.Application.Services;
using System.Security.Claims;

namespace RSpot.Booking.API.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var bookings = await _bookingService.GetBookingsForUserAsync(userId);
        return Ok(bookings);
    }
}
