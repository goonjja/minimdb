using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniMdb.Backend.Shared;

namespace MiniMdb.Backend.Controllers
{
    [Route("error")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<ApiMessage> Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if(exceptionHandlerPathFeature?.Error != null)
            {
                _logger.LogError(exceptionHandlerPathFeature?.Error, $"Exception in: {exceptionHandlerPathFeature?.Path}");
            }

            return StatusCode(500, ApiMessage.MakeError(1, "System error"));
        }
    }
}
