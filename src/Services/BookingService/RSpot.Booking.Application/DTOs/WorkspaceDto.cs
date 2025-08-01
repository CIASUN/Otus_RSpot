using System;
using System.Text.Json.Serialization;

namespace RSpot.Booking.Application.DTOs
{
    public class WorkspaceDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("organizationId")]
        public Guid OrganizationId { get; set; }

        [JsonPropertyName("title")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("view")]
        public string Location { get; set; } = null!;
    }
}
