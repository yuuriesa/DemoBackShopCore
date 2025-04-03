using DemoBackShopCore.Data;
using DemoBackShopCore.DTOs;
using DemoBackShopCore.Models;
using DemoBackShopCore.Services;
using DemoBackShopCore.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            List<CustomerResponseDTO> customersResponses = _services.GenerateListCustomersResponseDTO(customers: customers);

            return Ok(customersResponses);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Customer result = _services.GetById(id: id);

            if (result == null)
            {
                return NotFound(DomainResponseMessages.CustomerNotFoundMessageError);
            }

            CustomerResponseDTO customerResponse = _services.GenerateCustomerResponseDTO(customer: result);

            return Ok(customerResponse);
        }

        [HttpPost]
        public IActionResult Add(CustomerRequestDTO customerRequest)
        {
            ServiceResult<Customer> result = _services.Add(customerRequest: customerRequest);

            if (!result.Success)
            {
                return StatusCode(statusCode: result.StatusCode, value: result.Message);
            }

            _dbContext.SaveChanges();
            
            CustomerResponseDTO customerResponse = _services.GenerateCustomerResponseDTO(customer: result.Data);

            return CreatedAtAction(actionName: nameof(GetById), routeValues: new { id = customerResponse.CustomerId }, value: customerResponse);
        }

        [HttpPost("batch")]
        public async Task<IActionResult> AddBatch(List<CustomerRequestDTO> customers)
        {
            var transaction = _dbContext.Database.BeginTransaction();
            List<CustomerResponseDTO> customersReponse = new List<CustomerResponseDTO>();

            try
            {
                ServiceResult<List<Customer>> result = await _services.AddRange(customers: customers);

                if (!result.Success)
                {
                    return StatusCode(statusCode: result.StatusCode, value: result.Message);
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                foreach (var customer in result.Data)
                {
                    Customer getCustomerByEmail = _services.GetCustomerByEmail(emailAddress: customer.EmailAddress);
                    customersReponse.Add(item: _services.GenerateCustomerResponseDTO(customer: getCustomerByEmail));
                }
            }
            catch (Exception err)
            {
                await transaction.RollbackAsync();
                throw new Exception(err.Message);
            }
            
            return Ok(customersReponse);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CustomerRequestDTO customerRequestDTO)
        {
            ServiceResult<Customer> result = _services.Update(id: id, customerRequestDTO: customerRequestDTO);

            if (!result.Success)
            {
                return StatusCode(statusCode: result.StatusCode, value: result.Message);
            }

            _dbContext.SaveChanges();

            CustomerResponseDTO customerResponse = _services.GenerateCustomerResponseDTO(customer: result.Data);

            return Ok(customerResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            ServiceResult<Customer> result = _services.Remove(id: id);

            if (!result.Success)
            {
                return StatusCode(statusCode: result.StatusCode, value: result.Message);
            }

            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}