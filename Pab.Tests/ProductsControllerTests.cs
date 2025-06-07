using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace Pab.Tests
{
    public class ProductsControllerTests
      : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductsControllerTests(WebApplicationFactory<Program> factory) =>
          _client = factory.CreateClient();

        [Fact]
        public async Task GetProducts_ShouldReturn200AndJsonArray()
        {
            var response = await _client.GetAsync("/api/Products");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var arr = JsonSerializer.Deserialize<JsonElement>(content);
            arr.ValueKind.Should().Be(JsonValueKind.Array);
        }
    }
}
