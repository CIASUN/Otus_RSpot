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
    [SwaggerOperation(
        Summary = "Получить мои бронирования",
        Description = "Возвращает список всех бронирований для текущего аутентифицированного пользователя"
    )]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var bookings = await _bookingService.GetBookingsForUserAsync(userId);
        return Ok(bookings);
    }

    /// <summary>
    /// Создать новое бронирование.
    /// </summary>
    /// <param name="request">Данные для создания бронирования.</param>
    /// <returns>Результат операции.</returns>
    [HttpPost]
    [Authorize]
    [SwaggerOperation(
        Summary = "Создать бронирование",
        Description = "Создаёт новое бронирование рабочего места для текущего пользователя"
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
