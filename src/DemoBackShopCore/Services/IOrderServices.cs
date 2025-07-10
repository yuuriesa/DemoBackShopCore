using DemoBackShopCore.DTOs;
using DemoBackShopCore.Models;
using DemoBackShopCore.Utils;

namespace DemoBackShopCore.Services
{
    public interface IOrderServices
    {
        public IQueryable<Order> GetAll(PaginationFilter paginationFilter);
        public Order GetById(int id);
        public ServiceResult<Order> Add(OrderRequestDTO orderRequestDTO);
        public ServiceResult<Order> Update(int id, OrderRequestDTO orderRequestDTO);
        public ServiceResult<Order> Remove(int id);
        public OrderResponseDTO GenerateOrderResponseDTO(Order order);
    }
}