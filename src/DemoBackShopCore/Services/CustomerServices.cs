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

        public ServiceResult<Customer> Add(CustomerRequestDTO customerRequest)
        {
            Customer newCustomer = Customer.RegisterNew
                                (
                                    firstName: customerRequest.FirstName,
                                    lastName: customerRequest.LastName,
                                    emailAddress: customerRequest.EmailAddress,
                                    dateOfBirth: customerRequest.DateOfBirth
                                );
            
            if (!newCustomer.IsValid())
            {
                return ServiceResult<Customer>.ErrorResult(message: newCustomer.ErrorMessageIfIsNotValid, statusCode: 422);
            }

            _repository.Add(entity: newCustomer);
            return ServiceResult<Customer>.SuccessResult(data: newCustomer, statusCode: 201);
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