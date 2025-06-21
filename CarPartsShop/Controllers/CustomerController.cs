using CarPartsShop.Data;
using Microsoft.AspNetCore.Mvc;


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