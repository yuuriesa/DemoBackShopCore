using DemoBackShopCore.Data;
using DemoBackShopCore.DTOs;
using DemoBackShopCore.Models;
using DemoBackShopCore.Services;
using DemoBackShopCore.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackShopCore.Controllers
{
    [ApiController]
    [Route("api/Customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICustomerServices _services;
        public CustomersController(ApplicationDbContext dbContext, ICustomerServices services)
        {
            _dbContext = dbContext;
            _services = services;
        }

        [HttpGet]
        public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 0 || pageSize < 0) return BadRequest(DomainResponseMessages.CustomerPaginationError);

            PaginationFilter paginationFilter = new PaginationFilter(pageNumber: pageNumber, pageSize: pageSize);

            IQueryable<Customer> customers = _services.GetAll(paginationFilter: paginationFilter);

            if (customers.Count() == 0) return NoContent();

            return Ok(customers);
        }

        [HttpPost]
        public IActionResult Add(CustomerRequestDTO customerRequest)
        {
            ServiceResult<Customer> customer = _services.Add(customerRequest: customerRequest);

            if (!customer.Success)
            {
                return StatusCode(statusCode: customer.StatusCode, value: customer.Message);
            }

            _dbContext.SaveChanges();

            //por o CreatedAtAction aqui, depois de por o método do getbyid
            return Created("", customer);
        }
    }
}