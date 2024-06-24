using MongoDB.Bson;

namespace RecipeBook.Data.Entities
{
    public interface EntityBase
    {
        public string Id { get; set; }
    }
}
