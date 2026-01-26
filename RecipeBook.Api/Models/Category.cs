namespace RecipeBook.Api.Models
{
    public class Category
    {
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Color { get; set; }
        public int DisplayOrder { get; set; }
        public int RecipeCount { get; set; }  // Computed field
    }

    public class CategoryCreateRequest
    {
        public string Name { get; set; } = null!;
        public string? Color { get; set; }
    }

    public class CategoryUpdateRequest
    {
        public string Name { get; set; } = null!;
        public string? Color { get; set; }
        public int DisplayOrder { get; set; }
    }
}
