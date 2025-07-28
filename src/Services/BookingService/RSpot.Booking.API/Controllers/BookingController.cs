using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSpot.Booking.Application.DTOs;
using RSpot.Booking.Application.Interfaces;
using RSpot.Booking.Application.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace RSpot.Booking.API.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IBookingEventPublisher _eventPublisher;

    public BookingController(IBookingService bookingService, IBookingEventPublisher eventPublisher)
    {
        _bookingService = bookingService;
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// Получить список всех бронирований текущего пользователя.
    /// </summary>
    /// <returns>Список бронирований.</returns>
    [Authorize]
    [HttpGet("my")]
    [ProducesResponseType(typeof(IEnumerable<BookingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var bookings = await _bookingService.GetBookingsForUserAsync(userId);
        return Ok(bookings);
    }

    [HttpPost]
    [Authorize]
    [SwaggerOperation(Summary = "Создать бронирование", Description = "Создаёт новое бронирование рабочего места для текущего пользователя")]
    [SwaggerResponse(200, "Бронирование успешно создано")]
    [SwaggerResponse(400, "Ошибка при создании бронирования")]
    [SwaggerResponse(401, "Пользователь не авторизован")]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        try
        {
            var booking = await _bookingService.CreateBookingAsync(userId, request);

            await _eventPublisher.PublishBookingCreatedAsync(
                booking.Id.ToString(),
                booking.UserId.ToString(),
                booking.WorkspaceId,
                booking.StartTime,
                booking.EndTime
            );

            return Ok(new { message = "Бронирование успешно создано" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
