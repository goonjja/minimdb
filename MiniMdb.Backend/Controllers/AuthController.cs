using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MiniMdb.Auth;
using MiniMdb.Backend.Resources;
using MiniMdb.Backend.Shared;
using MiniMdb.Backend.ViewModels;
using System;
using System.Linq;

namespace MiniMdb.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : Controller
    {
        private static ApiUser[] Users = new[] {
            new ApiUser { Email  = "admin@example.com", Password = "m3g@pA$$W0rDDD", Roles = new [] {MiniMdbRoles.AdminRole } },
            new ApiUser { Email  = "anyone@example.com", Password = "w3@kpa$$w0rd", Roles =new [] {MiniMdbRoles.UserRole } }
        };

        private readonly JwtFactory _jwtFactory;
        private readonly IStringLocalizer<Errors> _localizer;

        public AuthController(JwtFactory jwtFactory, IStringLocalizer<Errors> localizer)
        {
            _jwtFactory = jwtFactory;
            _localizer = localizer;
        }

        /// <summary>
        /// Authenticate and get JWT token.
        /// Example users: 
        /// 1. Admin role, admin@example.com, m3g@pA$$W0rDDD
        /// 2. User role, anyone@example.com, w3@kpa$$w0rd
        /// </summary>
        /// <param name="req">Login and password</param>
        /// <returns>JWT Token or error</returns>
        [HttpPost]
        public ActionResult<ApiMessage<string>> Authenticate([FromBody] LoginRequest req)
        {
            Console.WriteLine(_localizer[ApiError.InvalidCredentials.Message]);
            var user = Users.FirstOrDefault(u => u.Email == req.Email && u.Password == req.Password);
            if (user == null)
                return BadRequest(new ApiMessage { Error = ApiError.InvalidCredentials.Localized(_localizer) });

            return ApiMessage.From(_jwtFactory.Generate(user.Email, user.Roles));
        }

        [HttpGet]
        [Route("test")]
        public ActionResult<ApiMessage> Test()
        {
            return BadRequest(new ApiMessage { Error = ApiError.InvalidCredentials.Localized(_localizer) });
        }

        /// <summary>
        /// Example method with exception cotnaining sensitive information
        /// </summary>
        [HttpGet]
        [Route("throw")]
        public ActionResult<ApiMessage<string>> FailAuth()
        {
            throw new System.Exception("It contains sensitive information: " + Users[0].Email);
        }

        private class ApiUser
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string[] Roles { get; set; }
        }
    }
}
