using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSpot.Users.Domain.Models
{
    public class WaitingList
    {
        public Guid WorkspaceId { get; set; }
        public Workspace Workspace { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

}
