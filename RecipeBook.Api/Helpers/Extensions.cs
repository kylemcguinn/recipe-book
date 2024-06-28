using System.Dynamic;

namespace RecipeBook.Api.Helpers
{
    public static class Extensions
    {
        public static string GetUserId(this HttpContext context)
        {
            return Constants.TestUserId;
        }

        public static bool DoesPropertyExist(this ExpandoObject expando, string name)
        {
            return (expando as IDictionary<string, object>).ContainsKey(name);
        }

        public static dynamic? GetPropertyIfExists(this ExpandoObject expando, string name)
        {
            if (!DoesPropertyExist(expando, name))
                return null;

            return (expando as IDictionary<string, object>)[name];
        }
    }
}
