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

        public Customer Add(CustomerRequestDTO customerRequest)
        {
            Customer customer = Customer.RegisterNew
                                (
                                    firstName: customerRequest.FirstName,
                                    lastName: customerRequest.LastName,
                                    emailAddress: customerRequest.EmailAddress,
                                    dateOfBirth: customerRequest.DateOfBirth
                                );
            
            // if (!customer.IsValid())
            // {
            //     return
            // }

            _repository.Add(entity: customer);
            return GetById(id: customer.CustomerId);
        }

        public void AddRange(IEnumerable<CustomerRequestDTO> customers)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Customer> GetAll(PaginationFilter paginationFilter)
        {
            return _repository.GetAll(paginationFilter: paginationFilter);
        }

        public Customer GetById(int id)
        {
            return _repository.GetById(id: id);
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