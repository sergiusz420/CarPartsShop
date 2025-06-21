using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CarPartsShop.Tests
{
    public class ProductIntegrationTests : IClassFixture<WebApplicationFactory<CarPartsShop.Program>>
    {
        private readonly HttpClient _client;

        public ProductIntegrationTests(WebApplicationFactory<CarPartsShop.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllProducts_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/product");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
