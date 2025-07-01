using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSpot.Booking.Infrastructure.Models
{
    public class Workspace
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }

        public string Title { get; set; } = null!;
        public int Floor { get; set; }
        public bool HasSocket { get; set; }
        public string View { get; set; } = null!;
        public bool IsQuietZone { get; set; }
        public int Capacity { get; set; }
        public string? Description { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<WaitingList> WaitingLists { get; set; } = new List<WaitingList>();
    }

}
