using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CarPartsShop.Tests
{
    public class ProductIntegrationTests(WebApplicationFactory<CarPartsShop.Program> factory) : IClassFixture<WebApplicationFactory<CarPartsShop.Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task GetAllProducts_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/product");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreated()
        {
            // Arrange
            var newProduct = new
            {
                name = "Nowy produkt",
                ean = "1234567890123",
                price = 199.99,
                stock = 5,
                sku = "SKU123",
                category = "Hamulce"
            };

            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(newProduct),
                System.Text.Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/product", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNoContent_WhenValid()
        {
            // Add product
            var originalProduct = new
            {
                name = "Stary produkt",
                ean = "1111111111111",
                price = 100.0,
                stock = 10,
                sku = "SKU111",
                category = "Silnik"
            };

            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(originalProduct),
                System.Text.Encoding.UTF8,
                "application/json");

            var postResponse = await _client.PostAsync("/api/product", content);
            postResponse.EnsureSuccessStatusCode();

            // Read new product ID
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var created = System.Text.Json.JsonSerializer.Deserialize<ProductResponse>(responseContent,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Product update
            var updatedProduct = new
            {
                id = created.Id,
                name = "Zaktualizowany produkt",
                ean = "9999999999999",
                price = 150.0,
                stock = 5,
                sku = "SKU999",
                category = "Zawieszenie"
            };

            var putContent = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(updatedProduct),
                System.Text.Encoding.UTF8,
                "application/json");

            var putResponse = await _client.PutAsync($"/api/product/{created.Id}", putContent);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);
        }

        public class ProductResponse
        {
            public int Id { get; set; }
        }


    }


}
