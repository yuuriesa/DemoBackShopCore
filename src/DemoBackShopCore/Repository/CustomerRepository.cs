using DemoBackShopCore.Data;
using DemoBackShopCore.Models;
using Microsoft.EntityFrameworkCore;

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
            Customer customer = _dbContext.Customers.AsNoTracking().Where(c => c.EmailAddress == emailAddress).Include(c => c.Addresses).FirstOrDefault();

            if (customer == null)
            {
                return null;
            }
            
            return customer;
        }
    }
}