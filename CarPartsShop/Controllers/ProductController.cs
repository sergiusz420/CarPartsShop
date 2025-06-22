using CarPartsShop.Data;
using CarPartsShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CarPartsShop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(TempDb.Products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = TempDb.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Product product)
        {
            product.Id = TempDb.Products.Count + 1;
            product.CreatedAt = DateTime.UtcNow;
            product.CreatedBy = Guid.NewGuid();
            TempDb.Products.Add(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, Product updated)
        {
            var product = TempDb.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            product.Name = updated.Name;
            product.Ean = updated.Ean;
            product.Price = updated.Price;
            product.Stock = updated.Stock;
            product.Sku = updated.Sku;
            product.Category = updated.Category;
            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedBy = Guid.NewGuid();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = TempDb.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            product.Deleted = true;
            return NoContent();
        }
    }
}

