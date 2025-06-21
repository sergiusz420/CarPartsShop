using Microsoft.AspNetCore.Mvc;
using CarPartsShop.Models;
using CarPartsShop.Data;


namespace CarPartsShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(TempDb.MockCustomer);
        }
    }
}