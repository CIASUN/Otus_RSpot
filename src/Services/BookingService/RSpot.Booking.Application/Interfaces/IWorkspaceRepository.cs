using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSpot.Booking.Application.DTOs;

namespace RSpot.Booking.Application.Interfaces
{
    public interface IWorkspaceRepository
    {
        Task<WorkspaceDto?> GetByIdAsync(Guid id);
    }

}
