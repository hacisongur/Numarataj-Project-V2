using Microsoft.AspNetCore.Mvc.Filters;

namespace Numarataj.WebUI.Filters
{
    public class NoCacheHeaderFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var headers = context.HttpContext.Response.Headers;

            if (!headers.ContainsKey("Cache-Control"))
            {
                headers.Add("Cache-Control", "no-store, no-cache, must-revalidate, max-age=0");
            }

            if (!headers.ContainsKey("Pragma"))
            {
                headers.Add("Pragma", "no-cache");
            }

            if (!headers.ContainsKey("Expires"))
            {
                headers.Add("Expires", "0");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
