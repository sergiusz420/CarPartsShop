using CarPartsShop.Data;
using CarPartsShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CarPartsShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCategoryController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(TempDb.Categories.Where(c => !c.Deleted));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = TempDb.Categories.FirstOrDefault(c => c.Id == id && !c.Deleted);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(ProductCategory category)
        {
            category.Id = TempDb.Categories.Count + 1;
            TempDb.Categories.Add(category);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, ProductCategory updated)
        {
            var category = TempDb.Categories.FirstOrDefault(c => c.Id == id && !c.Deleted);
            if (category == null) return NotFound();

            category.Name = updated.Name;
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = TempDb.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

            category.Deleted = true;
            return NoContent();
        }
    }
}
