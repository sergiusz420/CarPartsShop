using CarPartsShop.Controllers;
using CarPartsShop.Data;
using CarPartsShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarPartsShop.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void GetAll_ReturnsAllProducts()
        {
            // Arrange
            TempDb.Products =
            [
                new Product { Id = 1, Name = "Prod A" },
                new Product { Id = 2, Name = "Prod B" }
            ];

            var controller = new ProductController();

            // Act
            var result = controller.GetAll() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var products = result.Value as List<Product>;
            Assert.Equal(2, products.Count);
        }

        [Fact]
        public void GetById_ReturnsProduct_WhenProductExists()
        {
            // Arrange
            TempDb.Products =
    [
            new() { Id = 1, Name = "Test Product" }
    ];
            var controller = new ProductController();

            // Act
            var result = controller.GetById(1) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var product = result.Value as Product;
            Assert.Equal(1, product.Id);
        }

        [Fact]
        public void GetById_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            TempDb.Products = new List<Product>();
            var controller = new ProductController();

            // Act
            var result = controller.GetById(10);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
