using DemoBackShopCore.Data;
using DemoBackShopCore.Repository;

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
    }
}