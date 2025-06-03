using DemoBackShopCore.Models;

namespace DemoBackShopCore.Repository
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        public Product GetByCode(string code);
    }
}