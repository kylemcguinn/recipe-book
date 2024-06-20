using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace RecipeBook.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecipeImportController : ControllerBase
    {
        [HttpGet(Name = "ImportRecipe")]
        public async Task<IActionResult> Get(string url)
        {
            var json = await LoadRecipeFromUrl(url);
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
