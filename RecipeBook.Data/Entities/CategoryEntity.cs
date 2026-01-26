using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RecipeBook.Data.Entities
{
    public class CategoryEntity : EntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Color { get; set; }  // Hex color (e.g., "#3B82F6")
        public int DisplayOrder { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime? CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
    }
}
