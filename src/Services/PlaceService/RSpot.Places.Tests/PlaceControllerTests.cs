using Moq;
using Xunit;
using RSpot.Places.API.Controllers;
using RSpot.Places.Application.Interfaces;
using RSpot.Places.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RSpot.Places.Tests
{
    public class PlaceControllerTests
    {
        private readonly Mock<IPlaceRepository> _mockRepo;
        private readonly PlaceController _controller;

        public PlaceControllerTests()
        {
            _mockRepo = new Mock<IPlaceRepository>();
            _controller = new PlaceController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetWorkspaces_ReturnsOk_WithData()
        {
            // Arrange
            var expected = new List<Workspace>
            {
                new Workspace
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Test Workspace",
                    Floor = 3,
                    HasSocket = true,
                    View = "Park",
                    IsQuietZone = false,
                    Capacity = 5,
                    Description = "Quiet spot"
                }
            };
            _mockRepo.Setup(r => r.GetAllWorkspacesAsync()).ReturnsAsync(expected);

            // Act
            var result = await _controller.GetWorkspaces();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actual = Assert.IsAssignableFrom<List<Workspace>>(okResult.Value);
            Assert.Single(actual);
            Assert.Equal("Test Workspace", actual[0].Title);
        }

        [Fact]
        public async Task AddOrganization_ReturnsOk()
        {
            // Arrange
            var newOrg = new Organization { Id = Guid.NewGuid(), Name = "Test Org" };

            // Act
            var result = await _controller.AddOrganization(newOrg);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockRepo.Verify(r => r.AddOrganizationAsync(newOrg), Times.Once);
        }
    }
}
