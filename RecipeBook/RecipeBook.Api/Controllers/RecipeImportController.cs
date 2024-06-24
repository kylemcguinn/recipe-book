using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json.Linq;
using RecipeBook.Api.Helpers;
using RecipeBook.Data.Entities;
using RecipeBook.Data.Persistence;

namespace RecipeBook.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecipeImportController : BaseController
    {
        public RecipeImportController(IPersistence persistence) : base(persistence)
        {
        }

        [HttpGet(Name = "ImportRecipe")]
        public async Task<IActionResult> ImportRecipe(string url)
        {
            var userId = HttpContext.GetUserId();

            var json = await LoadRecipeFromUrl(url);

            var recipeJson = BsonSerializer.Deserialize<dynamic>(json);

            if (recipeJson == null)
            {
                throw new Exception("Unable to deserialize recipe json");
            }

            var recipe = new RecipeEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                RecipeJson = recipeJson,
                UserId = userId
            };

            await Persistence.Persist(recipe);

            return new ContentResult()
            {
                Content = json,
                ContentType = "application/json"
            };
        }

        private async Task<string> LoadRecipeFromUrl(string url)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = await web.LoadFromWebAsync(url);

            var ldJsonNodes = htmlDoc.DocumentNode.SelectNodes("//script[translate(@type, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = 'application/ld+json']");

            if (ldJsonNodes == null)
                throw new Exception("No recipe script block found");

            var recipeJson = ldJsonNodes.Where(x => x.InnerText.Contains("@type\":\"Recipe\"") || x.InnerText.Contains("@type\": \"Recipe\"")).FirstOrDefault();

            if (recipeJson == null)
                throw new Exception("No recipe found within script block");

            var json = recipeJson.InnerText;

            dynamic dynamicJson = JObject.Parse(json);
            var graph = dynamicJson["@graph"];
            if (graph != null) //If the graph attribute is present, then there are multiple json-ld objects bundled in this script block.
            {
                var graphObject = graph as JArray;
                var recipeObject = graphObject.First(x => x["@type"] != null && x["@type"].ToString() == "Recipe");
                json = recipeObject.ToString();
            }

            return json;
        }
    }
}
