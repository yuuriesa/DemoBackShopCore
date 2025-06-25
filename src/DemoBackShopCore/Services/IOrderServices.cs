using DemoBackShopCore.DTOs;
using DemoBackShopCore.Models;
using DemoBackShopCore.Utils;

namespace DemoBackShopCore.Services
{
    public interface IOrderServices
    {
        public IQueryable<Order> GetAll(PaginationFilter paginationFilter);
        public ServiceResult<Order> Add(OrderRequestDTO orderRequestDTO);
    }
}