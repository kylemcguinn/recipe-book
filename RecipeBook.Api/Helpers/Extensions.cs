namespace RecipeBook.Api.Helpers
{
    public static class Extensions
    {
        public static string GetUserId(this HttpContext context)
        {
            return Constants.TestUserId;
        }
    }
}
