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
        public string title { get; set; } = null!;
        public int floor { get; set; }
        public int description { get; set; }
    }
}
