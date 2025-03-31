using DemoBackShopCore.Models;

namespace DemoBackShopCore.Repository
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        public Customer GetCustomerByEmail(string emailAddress);
    }
}