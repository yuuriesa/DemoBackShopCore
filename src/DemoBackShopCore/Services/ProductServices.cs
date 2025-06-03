using DemoBackShopCore.Data;
using DemoBackShopCore.Models;
using DemoBackShopCore.Repository;
using DemoBackShopCore.Utils;

namespace DemoBackShopCore.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _repository;
        private readonly ApplicationDbContext _dbContext;

        public ProductServices
        (
            IProductRepository repository,
            ApplicationDbContext dbContext
        )
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public IQueryable<Product> GetAll(PaginationFilter paginationFilter)
        {
            return _repository.GetAll(paginationFilter: paginationFilter);
        }

        public Product GetById(int id)
        {
            return _repository.GetById(id: id);
        }
    }
}