using CarPartsShop.Controllers;
using CarPartsShop.Data;
using CarPartsShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarPartsShop.Tests
{
    public class CartControllerTests
    {
        [Fact]
        public void AddToCart_ReturnsCreated_WhenProductExists()
        {
            TempDb.Products = [new Product { Id = 1, Name = "Test Product" }];
            TempDb.CartItems = [];

            var controller = new CartController();

            var cartItem = new CartItem { ProductId = 1, Quantity = 2 };

            var result = controller.AddToCart(cartItem) as CreatedAtActionResult;

            Assert.NotNull(result);
            var added = result.Value as CartItem;
            Assert.Equal(1, added.ProductId);
            Assert.Equal(2, added.Quantity);
        }

        [Fact]
        public void AddToCart_ReturnsBadRequest_WhenProductIdInvalid()
        {
            TempDb.Products = [];
            TempDb.CartItems = [];

            var controller = new CartController();
            var cartItem = new CartItem { ProductId = 10, Quantity = 1 };

            var result = controller.AddToCart(cartItem);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateQuantity_ReturnsNoContent_WhenCartItemExists()
        {
            TempDb.CartItems = [new CartItem { Id = 1, ProductId = 1, Quantity = 1, Deleted = false }];

            var controller = new CartController();
            var updated = new CartItem { Quantity = 5 };

            var result = controller.UpdateQuantity(1, updated);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal(5, TempDb.CartItems.First().Quantity);
        }

        [Fact]
        public void ProcessCart_ReturnsReceipt_WhenItemsExist()
        {
            TempDb.CartItems = [
                new CartItem
                {
                    Id = 1,
                    ProductId = 1,
                    ProductName = "Produkt A",
                    Quantity = 2,
                    Price = 10,
                    CustomerEmail = "test@shop.pl",
                    Processed = false,
                    Deleted = false
                }
            ];
            TempDb.Receipts = [];

            var controller = new CartController();
            var user = new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity(
                    new[]
                    {
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "test@shop.pl")
                    },
                    "test"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var result = controller.ProcessCart() as OkObjectResult;

            Assert.NotNull(result);
            var receipt = result.Value as Receipt;
            Assert.NotNull(receipt);
            Assert.Single(receipt.Items);
            Assert.Equal(20, receipt.TotalAmount); // 2 * 10
        }

        [Fact]
        public void ProcessCart_ReturnsBadRequest_WhenCartEmpty()
        {
            TempDb.CartItems = [];
            TempDb.Receipts = [];

            var controller = new CartController();
            var user = new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity(
                    new[]
                    {
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "empty@shop.pl")
                    },
                    "test"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var result = controller.ProcessCart();

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
