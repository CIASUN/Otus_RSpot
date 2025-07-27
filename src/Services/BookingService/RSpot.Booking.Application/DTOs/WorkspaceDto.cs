using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSpot.Booking.Application.DTOs
{
    public class WorkspaceDto
    {
        public Guid Id { get; set; }
        public Guid Organization_id { get; set; }
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
    }
}
