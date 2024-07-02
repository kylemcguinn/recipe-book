namespace RecipeBook.Api.Models
{
    public class RecipeCard
    {
        public string? Id { get; set; } = null!;
        public string? Name { get; set; } = null!;
        public List<RecipeCardImage> Image { get; set; } = null!;
        public string? Url { get; set; } = null!;
        public string? Description { get; set; } = null!;
    }

    public class RecipeCardImage
    {
        public string Url { get; set; } = null!;
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
