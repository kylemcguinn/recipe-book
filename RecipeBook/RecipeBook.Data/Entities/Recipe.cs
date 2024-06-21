using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace RecipeBook.Data.Entities
{
    public class Recipe: EntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public BsonDocument RecipeJson { get; set; } = null!;
        public string UserId { get; set; } = null!;
    }
}
