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

        [HttpDelete("{id}", Name = "DeleteRecipeCard")]
        public async Task<IActionResult> DeleteRecipeCard(string id)
        {
            var userId = HttpContext.GetUserId();

            try
            {
                await Persistence.DeleteEntity<RecipeEntity>(id, userId);
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return NotFound(); // Don't reveal recipe exists but user doesn't own it
            }
        }

        [HttpPut("{id}/categories", Name = "UpdateRecipeCategories")]
        public async Task<IActionResult> UpdateRecipeCategories(string id, [FromBody] List<string> categoryIds)
        {
            var userId = HttpContext.GetUserId();
            var recipe = await Persistence.GetEntity<RecipeEntity>(id);

            if (recipe == null || recipe.UserId != userId)
                return NotFound();

            recipe.CategoryIds = categoryIds;
            recipe.UpdatedAtUtc = DateTime.UtcNow;

            await Persistence.Update(id, recipe);
            return Ok();
        }

        [HttpGet("grouped", Name = "GetRecipesGroupedByCategory")]
        public async Task<Dictionary<string, List<RecipeCard>>> GetRecipesGroupedByCategory()
        {
            var userId = HttpContext.GetUserId();

            // Get all recipes and categories
            var recipes = await Persistence.GetEntities<RecipeEntity>(x => x.UserId == userId);
            var categories = await Persistence.GetEntities<CategoryEntity>(x => x.UserId == userId);
            var categoryMap = categories.OrderBy(c => c.DisplayOrder)
                                        .ToDictionary(c => c.Id, c => c.Name);

            // Initialize groups
            var grouped = new Dictionary<string, List<RecipeCard>>();
            foreach (var category in categoryMap.Values)
            {
                grouped[category] = new List<RecipeCard>();
            }
            grouped["Uncategorized"] = new List<RecipeCard>();

            // Group recipes
            foreach (var recipe in recipes)
            {
                var card = new RecipeCard(recipe);

                if (recipe.CategoryIds == null || recipe.CategoryIds.Count == 0)
                {
                    grouped["Uncategorized"].Add(card);
                }
                else
                {
                    foreach (var catId in recipe.CategoryIds)
                    {
                        if (categoryMap.TryGetValue(catId, out var catName))
                        {
                            grouped[catName].Add(card);
                        }
                    }
                }
            }

            // Remove empty categories
            return grouped.Where(g => g.Value.Count > 0)
                          .ToDictionary(g => g.Key, g => g.Value);
        }
    }
}
