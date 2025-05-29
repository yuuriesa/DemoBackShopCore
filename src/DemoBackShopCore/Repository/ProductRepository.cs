using DemoBackShopCore.Data;
using DemoBackShopCore.Models;

namespace DemoBackShopCore.Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}