using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MiniMdb.Auth;
using MiniMdb.Backend.Controllers;
using MiniMdb.Backend.Shared;
using MiniMdb.Backend.ViewModels;
using System;
using System.Text;
using Xunit;

namespace MiniMdb.Testing.Unit
{
    public class AuthControllerTest
    {

        [Fact]
        public void TestSuccessfulAuthentication()
        {
            var jwtFactory = GetJwtFactory();
            var controller = new AuthController(jwtFactory, null);
            var result = controller.Authenticate(new LoginRequest { Email = AuthController.Users[0].Email, Password = AuthController.Users[0].Password });
            Assert.Null(result.Value.Error);
            Assert.NotNull(result.Value.Data);
            Assert.NotEmpty(result.Value.Data[0]);
        }

        private static JwtFactory GetJwtFactory() => new JwtFactory(new JwtSettings
        {
            ValidFor = TimeSpan.FromMinutes(10),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes("zNh23k84AyQ7wcrqoUevaYXgDKBpS5jC")),
                SecurityAlgorithms.HmacSha256
            )
        });

        [Fact]
        public void TestEmptyAuthentication()
        {
            var jwtFactory = GetJwtFactory();
            var controller = new AuthController(jwtFactory, null);
            var result = controller.Authenticate(new LoginRequest { });

            Assert.IsType<BadRequestObjectResult>(result.Result);
            var badRequest = result.Result as BadRequestObjectResult;
            Assert.IsType<ApiMessage>(badRequest.Value);
            var apiMessage = badRequest.Value as ApiMessage;

            Assert.NotNull(apiMessage.Error);
            Assert.Equal(ApiError.InvalidCredentials.Code, apiMessage.Error.Code);
        }

        [Fact]
        public void TestFailedAuthentication()
        {
            var jwtFactory = GetJwtFactory();
            var controller = new AuthController(jwtFactory, null);
            var result = controller.Authenticate(new LoginRequest { Email = "non-existing", Password = string.Empty });

            Assert.IsType<BadRequestObjectResult>(result.Result);
            var badRequest = result.Result as BadRequestObjectResult;
            Assert.IsType<ApiMessage>(badRequest.Value);
            var apiMessage = badRequest.Value as ApiMessage;

            Assert.NotNull(apiMessage.Error);
            Assert.Equal(ApiError.InvalidCredentials.Code, apiMessage.Error.Code);
        }
    }
}
