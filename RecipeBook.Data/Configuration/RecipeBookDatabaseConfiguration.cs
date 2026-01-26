namespace RecipeBook.Data.Configuration
{
    public class RecipeBookDatabaseConfiguration
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string RecipesCollectionName { get; set; } = null!;

        public string CategoriesCollectionName { get; set; } = null!;
    }
}
