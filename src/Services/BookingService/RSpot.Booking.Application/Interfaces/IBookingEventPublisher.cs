using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSpot.Booking.Application.Interfaces;

public interface IBookingEventPublisher
{
    Task PublishBookingCreatedAsync(string bookingId, string userId, Guid workspaceId, DateTime startTime, DateTime endTime);
}

