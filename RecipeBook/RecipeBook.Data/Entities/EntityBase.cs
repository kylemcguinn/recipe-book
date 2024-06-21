using MongoDB.Bson;

namespace RecipeBook.Data.Entities
{
    public interface EntityBase
    {
        public ObjectId Id { get; set; }
    }
}
