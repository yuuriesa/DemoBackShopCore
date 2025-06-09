using DemoBackShopCore.Data;
using DemoBackShopCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoBackShopCore.Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Product GetByCode(string code)
        {
            Product? product = _dbContext.Products.AsNoTracking().FirstOrDefault(p => p.Code == code);

            if (product == null)
            {
                return null;
            }

            return product;
        }
    }
}