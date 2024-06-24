using Microsoft.AspNetCore.Mvc;
using RecipeBook.Data.Persistence;

namespace RecipeBook.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        internal IPersistence Persistence { get; set; }

        public BaseController(IPersistence persistence)
        {
            Persistence = persistence;
        }
    }
}
