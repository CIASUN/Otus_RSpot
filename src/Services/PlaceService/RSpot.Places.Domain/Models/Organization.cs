using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSpot.Places.Domain.Models
{
    public class Organization
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public ICollection<Workspace> Workspaces { get; set; } = new List<Workspace>();
    }

}
