using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSpot.Booking.Infrastructure.Models
{
    public class WaitingList
    {
        public Guid Id { get; set; }
        public Guid WorkspaceId { get; set; }
        public Workspace Workspace { get; set; } = null!;

        public Guid UserId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

}
