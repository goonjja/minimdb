using Microsoft.AspNetCore.Mvc;
using MiniMdb.Auth;
using MiniMdb.Backend.Shared;
using MiniMdb.Backend.ViewModels;
using System.Linq;

namespace MiniMdb.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private static ApiUser[] Users = new[] {
            new ApiUser { Email  = "admin@example.com", Password = "m3g@pA$$W0rDDD", Roles = new [] {MiniMdbRoles.AdminRole } },
            new ApiUser { Email  = "anyone@example.com", Password = "w3@kpa$$w0rd", Roles =new [] {MiniMdbRoles.UserRole } }
        };

        private readonly JwtFactory _jwtFactory;

        public AuthController(JwtFactory jwtFactory)
        {
            _jwtFactory = jwtFactory;
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
            var user = Users.FirstOrDefault(u => u.Email == req.Email && u.Password == req.Password);
            if (user == null)
                return BadRequest(ApiMessage.MakeError(2, "Invalid credentials"));

            return ApiMessage.From(_jwtFactory.Generate(user.Email, user.Roles));
        }

        /// <summary>
        /// Example method with exception cotnaining sensitive information
        /// </summary>
        [HttpGet("/throw")]
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
