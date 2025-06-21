using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                new Product { Id = 1, Name = "Test A" },
                new Product { Id = 2, Name = "Test B" }
            ];

            var controller = new ProductController();

            // Act
            var result = controller.GetAll() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var products = result.Value as List<Product>;
            Assert.Equal(2, products.Count);
        }
    }
}
