using Microsoft.AspNetCore.Mvc;
using RecipeBook.Api.Helpers;
using RecipeBook.Api.Models;
using RecipeBook.Data.Entities;
using RecipeBook.Data.Persistence;

namespace RecipeBook.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecipeCardController : BaseController
    {
        public RecipeCardController(IPersistence persistence) : base(persistence)
        {
        }

        [HttpGet(Name = "GetRecipeCards")]
        public async Task<List<RecipeCard>> GetRecipeCards()
        {
            var userId = HttpContext.GetUserId();

            var recipes = await Persistence.GetEntities<RecipeEntity>(x => x.UserId == userId);
            var results = new List<RecipeCard>();

            foreach (var recipe in recipes)
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
                                Width = image.width,
                                Height = image.height
                            });
                        }
                    }
                }

                var recipeCard = new RecipeCard
                {
                    Name = recipe.RecipeJson.GetPropertyIfExists("name"),
                    Image = images,
                    Description = recipe.RecipeJson.GetPropertyIfExists("description"),
                    Url = recipe.RecipeJson.GetPropertyIfExists("url")
                };

                results.Add(recipeCard);
            }

            return results;
        }
    }
}
