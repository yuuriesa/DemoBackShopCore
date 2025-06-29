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
    }
}