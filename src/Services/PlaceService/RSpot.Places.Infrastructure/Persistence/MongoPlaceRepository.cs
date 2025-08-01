﻿using MongoDB.Bson;
using MongoDB.Driver;
using RSpot.Places.Application.Interfaces;
using RSpot.Places.Domain.Models;
using MongoDB.Bson;

namespace RSpot.Places.Infrastructure.Persistence;

public class MongoPlaceRepository : IPlaceRepository
{
    private readonly IMongoCollection<Workspace> _workspaces;
    private readonly IMongoCollection<Organization> _organizations;

    public MongoPlaceRepository(IMongoDatabase database)
    {
        _workspaces = database.GetCollection<Workspace>("workspaces");
        _organizations = database.GetCollection<Organization>("organizations");
    }

    public async Task<List<Workspace>> GetAllWorkspacesAsync() =>
        await _workspaces.Find(_ => true).ToListAsync();

    public async Task<List<Organization>> GetAllOrganizationsAsync() =>
        await _organizations.Find(_ => true).ToListAsync();

    public async Task AddWorkspaceAsync(Workspace workspace) =>
        await _workspaces.InsertOneAsync(workspace);

    public async Task AddOrganizationAsync(Organization organization) =>
        await _organizations.InsertOneAsync(organization);

    public async Task<Workspace?> GetWorkspaceByIdAsync(string id)
    {
        var filter = Builders<Workspace>.Filter.Eq(w => w.Id, id);
        return await _workspaces.Find(filter).FirstOrDefaultAsync();
    }
}
