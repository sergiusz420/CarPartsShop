using CarPartsShop.Data;
using CarPartsShop.DTOs;
using CarPartsShop.Models;
using CarPartsShop.Services;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            return Ok("You are better than others (because you have more rights) :).");
        }

        [Authorize]
        [HttpPut("edit")]
        public IActionResult EditAccount([FromBody] EditAccountDto dto)
        {
            var email = User.Identity?.Name;

            var user = TempDb.Customers.FirstOrDefault(c => c.Email == email);
            if (user == null) return NotFound("User does not exist.");

            user.FullName = dto.FullName;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.Password = dto.Password;
            }

            return Ok("Account details have been updated..");
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = TempDb.Customers.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null) return NotFound("User not found.");

            user.Password = dto.NewPassword;

            return Ok("Password has been changed");
        }
    }
}
