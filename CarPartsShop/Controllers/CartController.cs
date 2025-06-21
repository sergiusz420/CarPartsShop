using Microsoft.AspNetCore.Mvc;
using CarPartsShop.Models;
using CarPartsShop.Data;
using System.Linq;

namespace CarPartsShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(TempDb.CartItems.Where(ci => !ci.Deleted));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = TempDb.CartItems.FirstOrDefault(ci => ci.Id == id && !ci.Deleted);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public IActionResult AddToCart(CartItem item)
        {
            var product = TempDb.Products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product == null) return BadRequest("Invalid ProductId");

            item.Id = TempDb.CartItems.Count + 1;
            TempDb.CartItems.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateQuantity(int id, CartItem updated)
        {
            var item = TempDb.CartItems.FirstOrDefault(ci => ci.Id == id && !ci.Deleted);
            if (item == null) return NotFound();

            item.Quantity = updated.Quantity;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = TempDb.CartItems.FirstOrDefault(ci => ci.Id == id);
            if (item == null) return NotFound();

            item.Deleted = true;
            return NoContent();
        }
    }
}
