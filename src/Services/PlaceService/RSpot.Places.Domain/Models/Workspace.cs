﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace RSpot.Places.Domain.Models
{
    public class Workspace
    {
        [BsonId]
        public string Id { get; set; } = null!;

        [BsonElement("organizationId")]
        public string OrganizationId { get; set; } = null!;

        [BsonIgnore]
        public Organization Organization { get; set; } = null!;

        [Required]
        [BsonElement("name")]
        public string Title { get; set; } = null!;

        [BsonElement("location")]
        public string View { get; set; } = null!;

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
