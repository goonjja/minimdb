using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MiniMdb.Backend.Shared;
using System.Linq;

namespace MiniMdb.Backend.Helpers
{
    public class ValidRequestFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;

            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .Aggregate("", (s, a) => $"{a}, {s}");
            context.Result = new BadRequestObjectResult(ApiMessage.MakeError(1, errors));
        }
    }
}
