using Microsoft.AspNetCore.Mvc;
using RecipeBook.Api.Helpers;
using RecipeBook.Data.Entities;
using RecipeBook.Data.Persistence;

namespace RecipeBook.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        IPersistence Persistence { get; set; }

        public RecipeController(IPersistence persistence)
        {
            Persistence = persistence;
        }

        [HttpGet(Name = "GetRecipes")]
        public async Task<List<Recipe>> GetRecipes()
        {
            var userId = HttpContext.GetUserId();

            var recipes = await Persistence.GetEntities<Recipe>(x => x.UserId == userId);

            return recipes.ToList();
        }
    }
}
