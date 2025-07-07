namespace RSpot.Places.Application.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using RSpot.Places.Domain.Models;

    public interface IPlaceRepository
    {
        Task<List<Workspace>> GetAllWorkspacesAsync();
        Task<List<Organization>> GetAllOrganizationsAsync();

        Task AddWorkspaceAsync(Workspace workspace);
        Task AddOrganizationAsync(Organization organization);
    }
}
