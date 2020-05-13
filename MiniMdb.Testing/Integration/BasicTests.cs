using Microsoft.AspNetCore.Mvc.Testing;
using MiniMdb.Backend;
using System.Threading.Tasks;
using Xunit;

namespace MiniMdb.Testing.Integration
{
    public class BasicTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public BasicTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/auth/test")]
        public async Task Get_AuthTestReturnsBadRequest(string url)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}
