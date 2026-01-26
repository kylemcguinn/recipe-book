using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using RecipeBook.Api.Helpers;
using RecipeBook.Api.Models;
using RecipeBook.Data.Entities;
using RecipeBook.Data.Persistence;

namespace RecipeBook.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {
        public CategoryController(IPersistence persistence) : base(persistence)
        {
        }

        [HttpGet(Name = "GetCategories")]
        public async Task<List<Category>> GetCategories()
        {
            var userId = HttpContext.GetUserId();

            // Get all categories for user
            var categories = await Persistence.GetEntities<CategoryEntity>(x => x.UserId == userId);

            // Get all recipes for user to count recipes per category
            var recipes = await Persistence.GetEntities<RecipeEntity>(x => x.UserId == userId);

            var results = new List<Category>();

            foreach (var category in categories.OrderBy(c => c.DisplayOrder))
            {
                var recipeCount = recipes.Count(r => r.CategoryIds != null && r.CategoryIds.Contains(category.Id));

                results.Add(new Category
                {
                    Id = category.Id,
                    Name = category.Name,
                    Color = category.Color,
                    DisplayOrder = category.DisplayOrder,
                    RecipeCount = recipeCount
                });
            }

            return results;
        }

        [HttpPost(Name = "CreateCategory")]
        public async Task<Category> CreateCategory([FromBody] CategoryCreateRequest request)
        {
            var userId = HttpContext.GetUserId();

            // Get existing categories to determine next display order
            var existingCategories = await Persistence.GetEntities<CategoryEntity>(x => x.UserId == userId);
            var maxDisplayOrder = existingCategories.Any() ? existingCategories.Max(c => c.DisplayOrder) : -1;

            var category = new CategoryEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = request.Name,
                Color = request.Color,
                DisplayOrder = maxDisplayOrder + 1,
                UserId = userId,
                CreatedAtUtc = DateTime.UtcNow
            };

            await Persistence.Persist(category);

            return new Category
            {
                Id = category.Id,
                Name = category.Name,
                Color = category.Color,
                DisplayOrder = category.DisplayOrder,
                RecipeCount = 0
            };
        }

        [HttpPut("{id}", Name = "UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(string id, [FromBody] CategoryUpdateRequest request)
        {
            var userId = HttpContext.GetUserId();

            var category = await Persistence.GetEntity<CategoryEntity>(id);

            if (category == null || category.UserId != userId)
                return NotFound();

            category.Name = request.Name;
            category.Color = request.Color;
            category.DisplayOrder = request.DisplayOrder;
            category.UpdatedAtUtc = DateTime.UtcNow;

            await Persistence.Update(id, category);

            return Ok();
        }

        [HttpDelete("{id}", Name = "DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var userId = HttpContext.GetUserId();

            var category = await Persistence.GetEntity<CategoryEntity>(id);

            if (category == null || category.UserId != userId)
                return NotFound();

            // Remove this category from all recipes
            var recipes = await Persistence.GetEntities<RecipeEntity>(x => x.UserId == userId);

            foreach (var recipe in recipes)
            {
                if (recipe.CategoryIds != null && recipe.CategoryIds.Contains(id))
                {
                    recipe.CategoryIds.Remove(id);
                    recipe.UpdatedAtUtc = DateTime.UtcNow;
                    await Persistence.Update(recipe.Id, recipe);
                }
            }

            // Delete the category
            await Persistence.DeleteEntity<CategoryEntity>(id, userId);

            return Ok();
        }
    }
}
