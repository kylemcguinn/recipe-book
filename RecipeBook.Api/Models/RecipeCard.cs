using System.Dynamic;
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

            // Map ingredients
            var dynamicIngredients = recipe.RecipeJson.GetPropertyIfExists("recipeIngredient");
            if (dynamicIngredients != null)
            {
                RecipeIngredient = new List<string>();
                foreach (var ingredient in dynamicIngredients)
                {
                    RecipeIngredient.Add(ingredient.ToString());
                }
            }

            // Map instructions
            var dynamicInstructions = recipe.RecipeJson.GetPropertyIfExists("recipeInstructions");
            if (dynamicInstructions != null)
            {
                RecipeInstructions = new List<string>();
                foreach (var instruction in dynamicInstructions)
                {
                    if (instruction is string)
                    {
                        RecipeInstructions.Add(instruction);
                    }
                    else if (instruction is ExpandoObject instructionObj)
                    {
                        // Handle HowToStep objects with text property
                        var text = instructionObj.GetPropertyIfExists("text");
                        if (text != null)
                        {
                            RecipeInstructions.Add(text.ToString());
                        }
                    }
                }
            }

            // Map nutrition
            var dynamicNutrition = recipe.RecipeJson.GetPropertyIfExists("nutrition");
            if (dynamicNutrition is ExpandoObject nutritionObj)
            {
                Nutrition = new RecipeNutrition
                {
                    Calories = nutritionObj.GetPropertyIfExists("calories")?.ToString(),
                    UnsaturatedFatContent = nutritionObj.GetPropertyIfExists("unsaturatedFatContent")?.ToString(),
                    CarbohydrateContent = nutritionObj.GetPropertyIfExists("carbohydrateContent")?.ToString(),
                    CholesterolContent = nutritionObj.GetPropertyIfExists("cholesterolContent")?.ToString(),
                    FatContent = nutritionObj.GetPropertyIfExists("fatContent")?.ToString(),
                    FiberContent = nutritionObj.GetPropertyIfExists("fiberContent")?.ToString(),
                    ProteinContent = nutritionObj.GetPropertyIfExists("proteinContent")?.ToString(),
                    SaturatedFatContent = nutritionObj.GetPropertyIfExists("saturatedFatContent")?.ToString(),
                    SodiumContent = nutritionObj.GetPropertyIfExists("sodiumContent")?.ToString(),
                    SugarContent = nutritionObj.GetPropertyIfExists("sugarContent")?.ToString(),
                    TransFatContent = nutritionObj.GetPropertyIfExists("transFatContent")?.ToString()
                };
            }

            CategoryIds = recipe.CategoryIds ?? [];
        }

        public string? Id { get; set; } = null!;
        public string? Name { get; set; } = null!;
        public List<RecipeCardImage> Image { get; set; } = null!;
        public string? Url { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public List<string>? RecipeIngredient { get; set; }
        public List<string>? RecipeInstructions { get; set; }
        public RecipeNutrition? Nutrition { get; set; }
        public List<string> CategoryIds { get; set; } = [];
        public List<string>? SuggestedCategories { get; set; }  // Only populated during import
    }

    public class RecipeCardImage
    {
        public string Url { get; set; } = null!;
        public int Height { get; set; }
        public int Width { get; set; }
    }

    public class RecipeNutrition
    {
        public string? Calories { get; set; }
        public string? UnsaturatedFatContent { get; set; }
        public string? CarbohydrateContent { get; set; }
        public string? CholesterolContent { get; set; }
        public string? FatContent { get; set; }
        public string? FiberContent { get; set; }
        public string? ProteinContent { get; set; }
        public string? SaturatedFatContent { get; set; }
        public string? SodiumContent { get; set; }
        public string? SugarContent { get; set; }
        public string? TransFatContent { get; set; }
    }
}
