using RecipeBook.Api.Helpers;
using RecipeBook.Data.Entities;

namespace RecipeBook.Api.Models
{
    public class RecipeCard
    {
        public RecipeCard() { }
        public RecipeCard(RecipeEntity recipe)
        {
            var images = new List<RecipeCardImage>();

            var dynamicImages = recipe.RecipeJson.GetPropertyIfExists("image");

            if (dynamicImages != null)
            {
                foreach (var image in dynamicImages)
                {
                    if (image is string)
                    {
                        images.Add(new RecipeCardImage
                        {
                            Url = image
                        });
                    }
                    else
                    {
                        images.Add(new RecipeCardImage
                        {
                            Url = image.url,
                            Width = image.width is string widthStr ? int.TryParse(widthStr, out var w) ? w : 0 : (int)image.width,
                            Height = image.height is string heightStr ? int.TryParse(heightStr, out var h) ? h : 0 : (int)image.height
                        });
                    }
                }
            }

            Id = recipe.Id;
            Name = recipe.RecipeJson.GetPropertyIfExists("name");
            Image = images;
            Description = recipe.RecipeJson.GetPropertyIfExists("description");
            Url = recipe.RecipeJson.GetPropertyIfExists("url");
        }

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
