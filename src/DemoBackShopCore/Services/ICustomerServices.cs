using DemoBackShopCore.DTOs;
using DemoBackShopCore.Models;
using DemoBackShopCore.Repository;
using DemoBackShopCore.Utils;

namespace DemoBackShopCore.Services
{
    public interface ICustomerServices
    {
        public IQueryable<Customer> GetAll(PaginationFilter paginationFilter);
        public Customer GetById(int id);
        public void Add(CustomerRequestDTO customer);
        public void AddRange(IEnumerable<CustomerRequestDTO> customers);
        public void Update(int id, CustomerRequestDTO customer);
        public void Remove(int id);
    }
}