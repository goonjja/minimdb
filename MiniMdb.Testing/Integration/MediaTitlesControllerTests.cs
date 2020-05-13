using MiniMdb.Backend;
using MiniMdb.Backend.Controllers;
using MiniMdb.Backend.Shared;
using MiniMdb.Backend.ViewModels;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace MiniMdb.Testing.Integration
{
    public class MediaTitlesControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly CustomWebApplicationFactory<Startup> _factory;

        public MediaTitlesControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_ReturnsCorrectFirstPage()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/mediatitles?pageSize=5");
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var responseText = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(responseText);
            
            var page = JsonSerializer.Deserialize<ApiMessage<MediaTitleVm>>(responseText, _jsonOptions);
            Assert.NotNull(page.Data);
            Assert.NotNull(page.Pagination);
            Assert.Equal(1, page.Pagination.Page);
            Assert.Equal(3, page.Pagination.TotalPages);
            Assert.Equal(11, page.Pagination.Count);
            Assert.Equal(5, page.Data.Length);
            Assert.NotEmpty(page.Data[0].Name);
            Assert.NotEmpty(page.Data[0].Plot);
        }

        [Fact]
        public async Task Get_SearchCanFindInterstellar()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/mediatitles?nameFilter=interstellar");
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var responseText = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(responseText);

            var page = JsonSerializer.Deserialize<ApiMessage<MediaTitleVm>>(responseText, _jsonOptions);
            
            Assert.NotNull(page.Data);
            Assert.Single(page.Data);
            Assert.Equal("Interstellar", page.Data[0].Name);
            Assert.NotEmpty(page.Data[0].Plot);
        }

        [Fact]
        public async Task Get_SearchCantFindInterstellarInSeries()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/mediatitles?nameFilter=interstellar&typeFilter=1");
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var responseText = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(responseText);

            var page = JsonSerializer.Deserialize<ApiMessage<MediaTitleVm>>(responseText, _jsonOptions);

            Assert.NotNull(page.Data);
            Assert.Empty(page.Data);
        }

        [Fact]
        public async Task Get_FailsWithoutAuthentication()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/mediatitles/1");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Get_ReturnsForAuthenticatedUser()
        {
            var client = _factory.CreateClient();
            var token = await AuthenticateUser(AuthController.Users[0]);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var interstellar = await FindInterstellar(client);
            var response = await client.GetAsync($"/api/mediatitles/{interstellar.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseText = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiMessage<MediaTitleVm>>(responseText, _jsonOptions);
            Assert.Equal(interstellar.Id, result.Data[0].Id);
            Assert.Equal(interstellar.Name, result.Data[0].Name);
            Assert.Equal(interstellar.Plot, result.Data[0].Plot);
        }

        private async Task<MediaTitleVm> FindInterstellar(HttpClient client)
        {
            var response = await client.GetAsync("/api/mediatitles?nameFilter=interstellar");
            var responseText = await response.Content.ReadAsStringAsync();
            var page = JsonSerializer.Deserialize<ApiMessage<MediaTitleVm>>(responseText, _jsonOptions);
            return page.Data[0];
        }

        private async Task<string> AuthenticateUser(AuthController.ApiUser user)
        {
            var client = _factory.CreateClient();
            var data = JsonSerializer.Serialize(new LoginRequest
            {
                Email = user.Email,
                Password = user.Password
            }, _jsonOptions);

            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/auth", content);
            var result = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<ApiMessage<string>>(result, _jsonOptions);
            return token.Data[0];
        }
    }
}
