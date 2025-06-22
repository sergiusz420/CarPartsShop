using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CarPartsShop.Tests
{
    public class ProductIntegrationTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory = factory;

        [Fact]
        public async Task GetAllProducts_AsAdmin_ReturnsOk()
        {
            // Arrange
            var client = await GetAuthorizedClientAsync("admin@shop.pl", "admin123");

            // Act
            var response = await client.GetAsync("/api/product");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateProduct_AsAdmin_ReturnsCreated()
        {
            // Arrange
            var client = await GetAuthorizedClientAsync("admin@shop.pl", "admin123");

            var newProduct = new
            {
                name = "Nowy produkt",
                ean = "1234563",
                price = 199.99,
                stock = 5,
                sku = "SKU123",
                category = "Hamulce"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/product", newProduct);

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("CreateProduct RESPONSE: " + content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_AsAdmin_ReturnsNoContent()
        {
            // Arrange
            var client = await GetAuthorizedClientAsync("admin@shop.pl", "admin123");

            var originalProduct = new
            {
                name = "Produkt testowy",
                ean = "111111",
                price = 100.0,
                stock = 10,
                sku = "SKU001",
                category = "Test"
            };

            var postResp = await client.PostAsJsonAsync("/api/product", originalProduct);
            var postContent = await postResp.Content.ReadAsStringAsync();
            Console.WriteLine("POST: " + postContent);

            postResp.EnsureSuccessStatusCode();

            var created = JsonSerializer.Deserialize<ProductResponse>(postContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var updated = new
            {
                id = created!.Id,
                name = "Zmieniony produkt",
                ean = "2222222222222",
                price = 150.0,
                stock = 3,
                sku = "SKU002",
                category = "Zawieszenie"
            };

            // Act
            var putResponse = await client.PutAsJsonAsync($"/api/product/{created.Id}", updated);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);
        }

        private async Task<HttpClient> GetAuthorizedClientAsync(string email, string password)
        {
            var client = _factory.CreateClient();

            var loginData = new { Email = email, Password = password };
            var loginResponse = await client.PostAsJsonAsync("/api/auth/login", loginData);

            var loginBody = await loginResponse.Content.ReadAsStringAsync();
            Console.WriteLine("LOGIN RESPONSE: " + loginBody);

            loginResponse.EnsureSuccessStatusCode();

            var json = await loginResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            var token = json!["token"];

            var authClient = _factory.CreateClient();
            authClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return authClient;
        }

        public class ProductResponse
        {
            public int Id { get; set; }
        }
    }
}
