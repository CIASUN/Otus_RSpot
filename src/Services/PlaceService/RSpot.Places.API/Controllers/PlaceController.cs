using Microsoft.AspNetCore.Mvc;
using RSpot.Places.Application.Interfaces;
using RSpot.Places.Domain.Models;

namespace RSpot.Places.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaceController : ControllerBase
{
    private readonly IPlaceRepository _placeRepository;

    public PlaceController(IPlaceRepository placeRepository)
    {
        _placeRepository = placeRepository;
    }

    [HttpGet("workspaces")]
    public async Task<ActionResult<List<Workspace>>> GetWorkspaces()
    {
        var result = await _placeRepository.GetAllWorkspacesAsync();
        return Ok(result);
    }

    [HttpGet("organizations")]
    public async Task<ActionResult<List<Organization>>> GetOrganizations()
    {
        var result = await _placeRepository.GetAllOrganizationsAsync();
        return Ok(result);
    }

    [HttpPost("workspaces")]
    public async Task<IActionResult> AddWorkspace([FromBody] Workspace workspace)
    {
        await _placeRepository.AddWorkspaceAsync(workspace);
        return Ok();
    }

    [HttpPost("organizations")]
    public async Task<IActionResult> AddOrganization([FromBody] Organization organization)
    {
        await _placeRepository.AddOrganizationAsync(organization);
        return Ok();
    }
}
