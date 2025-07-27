using RSpot.Booking.Application.Interfaces;
using RSpot.Booking.Application.DTOs;
using RSpot.Booking.Domain.Models;

namespace RSpot.Booking.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IWorkspaceRepository _workspaceRepository;

        public BookingService(IBookingRepository bookingRepository, IWorkspaceRepository workspaceRepository)
        {
            _bookingRepository = bookingRepository;
            _workspaceRepository = workspaceRepository;
        }

        public async Task<IEnumerable<BookingDto>> GetBookingsForUserAsync(string userId)
        {
            var bookings = await _bookingRepository.GetByUserIdAsync(userId);

            var result = new List<BookingDto>();
            foreach (var booking in bookings)
            {
                var workspace = await _workspaceRepository.GetByIdAsync(booking.WorkspaceId);

                result.Add(new BookingDto
                {
                    Id = booking.Id,
                    Userid = booking.UserId,
                    WorkspaceId = booking.WorkspaceId,
                    StartTime = booking.StartTime,
                    EndTime = booking.EndTime,
                    Workspace = workspace
                });
            }

            return result;
        }

        public async Task CreateBookingAsync(string userId, CreateBookingRequest request)
        {
            if (!Guid.TryParse(userId, out var parsedUserId))
                throw new ArgumentException("Некорректный формат UserId");

            if (!Guid.TryParse(request.WorkspaceId, out var parsedWorkspaceId))
                throw new ArgumentException("Некорректный формат WorkspaceId");

            var booking = new RSpot.Booking.Domain.Models.Booking
            {
                Id = Guid.NewGuid(),
                UserId = parsedUserId,
                WorkspaceId = parsedWorkspaceId,
                StartTime = DateTime.SpecifyKind(request.StartTime, DateTimeKind.Utc),
                EndTime = DateTime.SpecifyKind(request.EndTime, DateTimeKind.Utc)
            };

            await _bookingRepository.AddAsync(booking);
        }
    }
}
