using DemoBackShopCore.Models;
using DemoBackShopCore.Services;
using DemoBackShopCore.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackShopCore.Controllers
{
    [ApiController]
    [Route("api/Product")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _services;
        
        public ProductsController(IProductServices services)
        {
            _services = services;
        }

        [HttpGet]
        public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 0 || pageSize < 0) return BadRequest(DomainResponseMessages.CustomerPaginationError);

            PaginationFilter paginationFilter = new PaginationFilter(pageNumber: pageNumber, pageSize: pageSize);

            IQueryable<Product> products = _services.GetAll(paginationFilter: paginationFilter);

            if (products.Count() == 0) return NoContent();

            return Ok(products);
        }
    }
}