using CarPartsShop.Controllers;
using CarPartsShop.Data;
using CarPartsShop.Models;
using Microsoft.AspNetCore.Mvc;



namespace CarPartsShop.Tests
{
    public class CartControllerTests
    {
        [Fact]
        public void AddToCart_ReturnsCreated_WhenProductExists()
        {
            // Arrange
            TempDb.Products =
            [
                new Product { Id = 1, Name = "Test Product" }
            ];
            TempDb.CartItems = [];

            var controller = new CartController();

            var cartItem = new CartItem
            {
                ProductId = 1,
                Quantity = 2
            };

            // Act
            var result = controller.AddToCart(cartItem) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            var added = result.Value as CartItem;
            Assert.Equal(1, added.ProductId);
            Assert.Equal(2, added.Quantity);
        }

        [Fact]
        public void AddToCart_ReturnsBadRequest_WhenProductIdInvalid()
        {
            // Arrange
            TempDb.Products = []; 
            TempDb.CartItems = [];

            var controller = new CartController();

            var cartItem = new CartItem
            {
                ProductId = 10,
                Quantity = 1
            };

            // Act
            var result = controller.AddToCart(cartItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}