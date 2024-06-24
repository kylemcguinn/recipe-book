using Microsoft.AspNetCore.Mvc;
using RecipeBook.Api.Helpers;
using RecipeBook.Data.Entities;
using RecipeBook.Data.Persistence;

namespace RecipeBook.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecipeController : BaseController
    {

        public RecipeController(IPersistence persistence) : base(persistence)
        {
        }

        [HttpGet(Name = "GetRecipe")]
        public async Task<List<RecipeEntity>> GetRecipe(string? id)
        {
            var userId = HttpContext.GetUserId();

            var recipes = await Persistence.GetEntities<RecipeEntity>(x => x.UserId == userId && (id == null || x.Id == id));

            return recipes.ToList();
        }
    }
}
