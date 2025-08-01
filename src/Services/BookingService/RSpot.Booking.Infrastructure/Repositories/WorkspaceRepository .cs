using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using RSpot.Booking.Application.DTOs;
using RSpot.Booking.Application.Interfaces;
using RSpot.Booking.Domain.Models;

namespace RSpot.Booking.Infrastructure.Repositories
{
    public class WorkspaceRepository : IWorkspaceRepository
    {
        private readonly IMongoCollection<Workspace> _collection;

        public WorkspaceRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Workspace>("workspaces");
        }

        public async Task<WorkspaceDto?> GetByIdAsync(Guid id)
        {
            var idString = id.ToString();

            var workspace = await _collection
                .Find(w => w.Id == idString)
                .FirstOrDefaultAsync();

            if (workspace == null) return null;

            return new WorkspaceDto
            {
                Id = Guid.Parse(workspace.Id),
                Name = workspace.Name,
                OrganizationId = workspace.OrganizationId,
                Location = workspace.Location
            };
        }


    }

}
