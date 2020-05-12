using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniMdb.Backend.Shared;

namespace MiniMdb.Backend.Controllers
{
    [Route("error")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<ApiMessage> Error()
        {
            return StatusCode(500, ApiMessage.MakeError(1, "System error"));
        }
    }
}
