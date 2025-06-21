using CarPartsShop.Controllers;
using CarPartsShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarPartsShop.Tests
{
    public class CustomerControllerTests
    {
        [Fact]
        public void GetCustomer_ReturnsMockCustomer()
        {
            // Arrange
            var controller = new CustomerController();

            // Act
            var result = controller.Get() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var customer = result.Value as Customer;
            Assert.Equal("Jan Kowalski", customer.FullName);
        }
    }
}
