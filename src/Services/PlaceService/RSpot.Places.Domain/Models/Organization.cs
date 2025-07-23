using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace RSpot.Places.Domain.Models
{
    public class Organization
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [Required]
        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("ownerUserId")]
        public string OwnerUserId { get; set; } = null!;

        [BsonIgnore]
        public ICollection<Workspace> Workspaces { get; set; } = new List<Workspace>();
    }
}
