using DemoBackShopCore.Data;
using DemoBackShopCore.DTOs;
using DemoBackShopCore.Models;
using DemoBackShopCore.Repository;
using DemoBackShopCore.Utils;

namespace DemoBackShopCore.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _repository;
        private readonly ApplicationDbContext _dbContext;

        public OrderServices
        (
            ApplicationDbContext dbContext,
            IOrderRepository repository
        )
        {
            _dbContext = dbContext;
            _repository = repository;

        }

        public ServiceResult<Order> Add(OrderRequestDTO orderRequestDTO)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Order> GetAll(PaginationFilter paginationFilter)
        {
            return _repository.GetAll(paginationFilter: paginationFilter);
        }
    }
}