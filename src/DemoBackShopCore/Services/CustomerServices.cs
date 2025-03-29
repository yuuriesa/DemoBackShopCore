using DemoBackShopCore.DTOs;
using DemoBackShopCore.Models;
using DemoBackShopCore.Repository;
using DemoBackShopCore.Utils;

namespace DemoBackShopCore.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly ICustomerRepository _repository;

        public CustomerServices(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public void Add(CustomerRequestDTO customer)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<CustomerRequestDTO> customers)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Customer> GetAll(PaginationFilter paginationFilter)
        {
            throw new NotImplementedException();
        }

        public Customer GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, CustomerRequestDTO customer)
        {
            throw new NotImplementedException();
        }
    }
}