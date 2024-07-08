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
                var recipeCard = new RecipeCard(recipe);

                results.Add(recipeCard);
            }

            return results;
        }
    }
}
