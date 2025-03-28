using DemoBackShopCore.Data;
using DemoBackShopCore.Models;

namespace DemoBackShopCore.Repository
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}