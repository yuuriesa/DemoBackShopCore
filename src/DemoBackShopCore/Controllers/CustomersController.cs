using DemoBackShopCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackShopCore.Controllers
{
    [ApiController]
    [Route("api/Customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerServices _services;
        public CustomersController(ICustomerServices services)
        {
            _services = services;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok();
        }
    }
}