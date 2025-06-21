using CarPartsShop.Data;
using CarPartsShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarPartsShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register([FromBody] Customer customer)
        {
            if (TempDb.Customers.Any(u => u.Email == customer.Email))
                return BadRequest("User with that email already exists");

            customer.Id = TempDb.Customers.Count + 1;
            TempDb.Customers.Add(customer);

            return Created("", new
            {
                customer.Id,
                customer.Email,
                customer.FullName,
                customer.Role
            });
        }
    }
}
