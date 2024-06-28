using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Dynamic;


namespace RecipeBook.Data.Entities
{
    public class RecipeEntity: EntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public ExpandoObject RecipeJson { get; set; } = null!;
        public string UserId { get; set; } = null!;
    }
}
