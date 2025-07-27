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
            _collection = database.GetCollection<Workspace>("workspace");
        }

        public async Task<WorkspaceDto?> GetByIdAsync(Guid id)
        {
            var workspace = await _collection.Find(w => w.Id == id).FirstOrDefaultAsync();
            if (workspace == null) return null;

            return new RSpot.Booking.Application.DTOs.WorkspaceDto
            {
                Id = workspace.Id,
                Name = workspace.Name,
                Organization_id = workspace.OrganizationId,
                Location = workspace.Location
                //Capacity = workspace.Capacity,
                //Floor = workspace.Floor,
                //HasSocket = workspace.HasSocket,
                //IsQuietZone = workspace.IsQuietZone,
                //Description = workspace.Description
            };
        }

    }

}
