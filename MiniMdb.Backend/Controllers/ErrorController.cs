using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MiniMdb.Backend.Resources;
using MiniMdb.Backend.Shared;

namespace MiniMdb.Backend.Controllers
{
    [Route("error")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;
        private readonly IStringLocalizer _localizer;
        private readonly bool _isStaging2;

        public ErrorController(ILogger<ErrorController> logger, IWebHostEnvironment env, IStringLocalizer<Errors> localizer)
        {
            _logger = logger;
            _localizer = localizer;
            _isStaging2 = env.IsStaging2();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<ApiMessage> Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature?.Error != null)
            {
                _logger.LogError(exceptionHandlerPathFeature?.Error, $"Exception in: {exceptionHandlerPathFeature?.Path}");
            }

            if (_isStaging2)
            {
                _logger.LogDebug("Additional info?");
            }

            return StatusCode(500, new ApiMessage { Error = ApiError.SystemError.Localized(_localizer) });
        }
    }
}
