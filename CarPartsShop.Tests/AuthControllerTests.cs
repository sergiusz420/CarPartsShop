using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CarPartsShop.Tests.Controllers
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public AuthControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task RegisterUser_WithUniqueEmail_ReturnsCreated()
        {
            // Arrange
            var email = $"user{Guid.NewGuid()}@test.pl";
            var registerDto = new
            {
                Email = email,
                Password = "test123",
                FullName = "Test Test",
                Role = "User"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("REGISTER RESPONSE: " + content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsJwtToken()
        {
            // Arrange
            var email = $"user{Guid.NewGuid()}@test.pl";
            var password = "test123";

            var register = new
            {
                Email = email,
                Password = password,
                FullName = "Login Test",
                Role = "User"
            };

            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", register);
            registerResponse.EnsureSuccessStatusCode();

            var loginDto = new
            {
                Email = email,
                Password = password
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.True(result!.ContainsKey("token"));
            Assert.False(string.IsNullOrWhiteSpace(result["token"]));
        }

        [Fact]
        public async Task Login_Admin_And_PostProduct_ReturnsCreated()
        {
            // Arrange
            var loginDto = new
            {
                Email = "admin@shop.pl",
                Password = "admin123"
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            loginResponse.EnsureSuccessStatusCode();

            var tokenData = await loginResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            var token = tokenData!["token"];

            var authorizedClient = _factory.CreateClient();
            authorizedClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var product = new
            {
                name = "Produkt admina",
                ean = "999",
                price = 300,
                stock = 15,
                sku = "ADMIN01",
                category = "Test"
            };

            // Act
            var postResponse = await authorizedClient.PostAsJsonAsync("/api/product", product);
            var body = await postResponse.Content.ReadAsStringAsync();
            Console.WriteLine("ADMIN POST PRODUCT RESPONSE: " + body);

            // Assert
            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
        }
    }
}
