using CarPartsShop.Data;
using CarPartsShop.Models;
using CarPartsShop.Services;
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

        [HttpPost("login")]
        public IActionResult Login([FromBody] Customer login)
        {
            var user = TempDb.Customers.FirstOrDefault(u =>
                u.Email == login.Email && u.Password == login.Password);

            if (user == null)
                return Unauthorized("Incorrect email or password");

            var token = TokenService.GenerateToken(user);

            return Ok(new
            {
                token,
                user.Email,
                user.FullName,
                user.Role
            });
        }
    }
}
