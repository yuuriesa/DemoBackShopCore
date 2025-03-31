using DemoBackShopCore.Data;
using DemoBackShopCore.Models;

namespace DemoBackShopCore.Repository
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Customer GetCustomerByEmail(string emailAddress)
        {
            Customer customer = _dbContext.Customers.FirstOrDefault(c => c.EmailAddress == emailAddress);

            if (customer == null)
            {
                return null;
            }
            
            return customer;
        }
    }
}