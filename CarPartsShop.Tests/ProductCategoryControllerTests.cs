using CarPartsShop.Controllers;
using CarPartsShop.Models;
using Microsoft.AspNetCore.Mvc;
using CarPartsShop.Data;

namespace CarPartsShop.Tests
{
    public class ProductCategoryControllerTests
    {
        [Fact]
        public void CreateCategory_ReturnsCreatedCategory()
        {
            // Arrange
            TempDb.Categories = [];

            var controller = new ProductCategoryController();
            var category = new ProductCategory { Name = "New Category" };

            // Act
            var result = controller.Create(category) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            var created = result.Value as ProductCategory;
            Assert.Equal("New Category", created.Name);
        }

        [Fact]
        public void DeleteCategory_MarksAsDeleted()
        {
            // Arrange
            TempDb.Categories =
            [
                new ProductCategory { Id = 1, Name = "To Be Deleted" }
            ];

            var controller = new ProductCategoryController();

            // Act
            var result = controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.True(TempDb.Categories[0].Deleted);
        }
    }
}
