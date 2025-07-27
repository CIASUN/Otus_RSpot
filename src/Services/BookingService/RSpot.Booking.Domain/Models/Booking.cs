using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSpot.Booking.Domain.Models
{
    [Table("booking")]
    public class Booking
    {
        [Column("id")]
        public Guid Id { get; set; }
        
        [Column("workspace_id")]
        public Guid WorkspaceId { get; set; }

        [Column("user_id")] 
        public Guid UserId { get; set; }

        [Column("start_time")] 
        public DateTime StartTime { get; set; }
        
        [Column("end_time")] 
        public DateTime EndTime { get; set; }
    }

}
