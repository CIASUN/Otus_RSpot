using System;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace RSpot.Booking.Domain.Models
{
    public class Workspace
    {
        [BsonId]
        public string Id { get; set; } = null!;

        [BsonRepresentation(BsonType.String)]
        [BsonElement("organizationId")]
        public Guid OrganizationId { get; set; }

        [Required]
        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("location")]
        public string Location { get; set; } = null!;

        [BsonElement("capacity")]
        [Range(1, 100)]
        public int Capacity { get; set; }

        [BsonElement("floor")]
        public int Floor { get; set; }

        [BsonElement("hasSocket")]
        public bool HasSocket { get; set; }

        [BsonElement("isQuietZone")]
        public bool IsQuietZone { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }
    }
}
