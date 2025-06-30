using DemoBackShopCore.Data;
using DemoBackShopCore.DTOs;
using DemoBackShopCore.Models;
using DemoBackShopCore.Services;
using DemoBackShopCore.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackShopCore.Controllers
{
    [ApiController]
    [Route("api/Orders")]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IOrderServices _services;

        public OrdersController
        (
            ApplicationDbContext dbContext,
            IOrderServices services
        )
        {
            _dbContext = dbContext;
            _services = services;
        }

        [HttpGet]
        public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 0 || pageSize < 0) return BadRequest(DomainResponseMessages.CustomerPaginationError);

            PaginationFilter paginationFilter = new PaginationFilter(pageNumber: pageNumber, pageSize: pageSize);

            IQueryable<Order> orders = _services.GetAll(paginationFilter: paginationFilter);

            if (orders.Count() == 0) return NoContent();

            List<OrderResponseDTO> orderResponseDTOs = new List<OrderResponseDTO>();

            foreach (var order in orders)
            {
                OrderResponseDTO orderResponseDTO = _services.GenerateOrderResponseDTO(order: order);
                orderResponseDTOs.Add(orderResponseDTO);
            }

            return Ok(orderResponseDTOs);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Order order = _services.GetById(id: id);

            if (order == null)
            {
                return NotFound(DomainResponseMessages.OrderNotFoundMessageError);
            }

            OrderResponseDTO orderResponseDTO = _services.GenerateOrderResponseDTO(order: order);

            return Ok(orderResponseDTO);
        }


        [HttpPost]
        public IActionResult Add(OrderRequestDTO orderRequestDTO)
        {
            ServiceResult<Order> result = _services.Add(orderRequestDTO: orderRequestDTO);

            if (!result.Success)
            {
                return StatusCode(statusCode: result.StatusCode, value: result.Message);
            }

            _dbContext.SaveChanges();

            OrderResponseDTO orderResponseDTO = _services.GenerateOrderResponseDTO(order: result.Data);

            return CreatedAtAction(actionName: nameof(GetById), routeValues: new { id = orderResponseDTO.orderId }, value: orderResponseDTO);
        }
    }
}