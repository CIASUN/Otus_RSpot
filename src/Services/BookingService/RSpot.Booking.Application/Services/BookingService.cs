using RSpot.Booking.Application.Interfaces;
using RSpot.Booking.Application.DTOs;
using RSpot.Booking.Domain.Models;
using System.Net.Http.Json;
using System.Net.Http;

namespace RSpot.Booking.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly HttpClient _httpClient;

        public BookingService(IBookingRepository bookingRepository, IHttpClientFactory httpClientFactory)
        {
            _bookingRepository = bookingRepository;
            _httpClient = httpClientFactory.CreateClient("PlaceService");
        }

        public async Task<IEnumerable<BookingDto>> GetBookingsForUserAsync(string userId)
        {
            var bookings = await _bookingRepository.GetByUserIdAsync(userId);
            var result = new List<BookingDto>();

            foreach (var booking in bookings)
            {
                var workspace = await GetWorkspaceFromPlaceServiceAsync(booking.WorkspaceId.ToString());

                result.Add(new BookingDto
                {
                    Id = booking.Id,
                    UserId = booking.UserId,
                    WorkspaceId = booking.WorkspaceId,
                    StartTime = booking.StartTime,
                    EndTime = booking.EndTime,
                    Workspace = workspace
                });
            }

            return result;
        }

        private async Task<WorkspaceDto?> GetWorkspaceFromPlaceServiceAsync(string workspaceId)
        {
            var response = await _httpClient.GetAsync($"/api/place/workspaces/{workspaceId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<WorkspaceDto>();
                return content;
            }

            // Логгирование ошибки можно добавить
            return null;
        }

        public async Task<BookingDto> CreateBookingAsync(string userId, CreateBookingRequest request)
        {
            if (!Guid.TryParse(userId, out var parsedUserId))
                throw new ArgumentException("Некорректный формат UserId");

            if (!Guid.TryParse(request.WorkspaceId, out var parsedWorkspaceId))
                throw new ArgumentException("Некорректный формат WorkspaceId");

            request.StartTime = DateTime.SpecifyKind(request.StartTime, DateTimeKind.Utc);
            request.EndTime = DateTime.SpecifyKind(request.EndTime, DateTimeKind.Utc);

            var booking = new RSpot.Booking.Domain.Models.Booking
            {
                Id = Guid.NewGuid(),
                UserId = parsedUserId,
                WorkspaceId = parsedWorkspaceId,
                StartTime = request.StartTime,
                EndTime = request.EndTime
            };

            await _bookingRepository.AddAsync(booking);

            return new BookingDto
            {
                Id = booking.Id,
                UserId = booking.UserId,
                WorkspaceId = booking.WorkspaceId,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime
            };
        }

    }
}
