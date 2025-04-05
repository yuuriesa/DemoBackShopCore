using DemoBackShopCore.Data;
using DemoBackShopCore.DTOs;
using DemoBackShopCore.Models;
using DemoBackShopCore.Repository;
using DemoBackShopCore.Utils;
using Microsoft.EntityFrameworkCore;

namespace DemoBackShopCore.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly ICustomerRepository _repository;
        private readonly ApplicationDbContext _dbContext;

        public CustomerServices(ICustomerRepository repository, ApplicationDbContext dbContext)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public ServiceResult<Customer> Add(CustomerRequestDTO customerRequest)
        {
            Customer customerExists = GetCustomerByEmail(emailAddress: customerRequest.EmailAddress);

            if (customerExists != null)
            {
                return ServiceResult<Customer>.ErrorResult
                (
                    message: $"{DomainResponseMessages.CustomerEmailExistsError}: {customerExists.EmailAddress}", statusCode: 409
                );
            }

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

        public async Task<ServiceResult<List<Customer>>> AddBatch(IEnumerable<CustomerRequestDTO> customers)
        {
            IEnumerable<string> duplicateEmails = customers.GroupBy(c => c.EmailAddress).Where(c => c.Count() > 1).Select(c => c.Key);

            if (duplicateEmails.Any())
            {
                return ServiceResult<List<Customer>>.ErrorResult
                (
                    message: DomainResponseMessages.DuplicateEmailError,
                    statusCode: 400
                );
            }

            List<Customer> newListCustomers = new List<Customer>();

            foreach (var customer in customers)
            {
                Customer customerExists = GetCustomerByEmail(emailAddress: customer.EmailAddress);

                if (customerExists != null)
                {
                    return ServiceResult<List<Customer>>.ErrorResult
                    (
                        message: $"{DomainResponseMessages.CustomerEmailExistsError}: {customerExists.EmailAddress}",
                        statusCode: 409
                    );
                }
                
                Customer newCustomer = Customer.RegisterNew
                (
                    firstName: customer.FirstName,
                    lastName: customer.LastName,
                    emailAddress: customer.EmailAddress,
                    dateOfBirth: customer.DateOfBirth
                );

                if (!newCustomer.IsValid())
                {
                    return ServiceResult<List<Customer>>.ErrorResult
                    (
                        message: $"{newCustomer.ErrorMessageIfIsNotValid} - customer: {newCustomer.EmailAddress}",
                        statusCode: 422
                    );
                }

                newListCustomers.Add(item: newCustomer);
            }

            _repository.AddRange(entities: newListCustomers);
            return ServiceResult<List<Customer>>.SuccessResult(data: newListCustomers);
        }

        public IQueryable<Customer> GetAll(PaginationFilter paginationFilter)
        {
            return _repository.GetAll(paginationFilter: paginationFilter);
        }

        public Customer GetById(int id)
        {
            return _repository.GetById(id: id);
        }

        public ServiceResult<Customer> Remove(int id)
        {
            Customer customer = GetById(id: id);
            if (customer == null)
            {
                return ServiceResult<Customer>.ErrorResult(message: DomainResponseMessages.CustomerNotFoundMessageError, statusCode: 404);
            }

            _repository.Remove(id: id);
            return ServiceResult<Customer>.SuccessResult(data: customer);
        }

        public ServiceResult<Customer> Update(int id, CustomerRequestDTO customerRequestDTO)
        {
            Customer getCustomerByEmail = _dbContext.Customers.AsNoTracking().FirstOrDefault(c => c.EmailAddress == customerRequestDTO.EmailAddress);

            if (getCustomerByEmail != null && getCustomerByEmail.CustomerId != id)
            {
                return ServiceResult<Customer>.ErrorResult
                    (
                        message: DomainResponseMessages.CustomerEmailExistsError,
                        statusCode: 409
                    ); 
            }

            Customer customerExists = _dbContext.Customers.AsNoTracking().FirstOrDefault(c => c.CustomerId == id);

            if (customerExists == null)
            {
                return ServiceResult<Customer>.ErrorResult
                    (
                        message: DomainResponseMessages.CustomerNotFoundMessageError,
                        statusCode: 404
                    ); 
            }

            Customer updatedCustomer = Customer.SetExistingInfo
            (
                customerId: customerExists.CustomerId,
                firstName: customerRequestDTO.FirstName,
                lastName: customerRequestDTO.LastName,
                emailAddress: customerRequestDTO.EmailAddress,
                dateOfBirth: DateOnly.FromDateTime(customerRequestDTO.DateOfBirth)
            );

            if (!updatedCustomer.IsValid())
            {
                return ServiceResult<Customer>.ErrorResult
                (
                    message: updatedCustomer.ErrorMessageIfIsNotValid,
                    statusCode: 422
                );
            }

            _repository.Update(id: id, entity: updatedCustomer);

            return ServiceResult<Customer>.SuccessResult(data: updatedCustomer);
        }

        public CustomerResponseDTO GenerateCustomerResponseDTO(Customer customer)
        {
            CustomerResponseDTO customerResponse = new CustomerResponseDTO
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                EmailAddress = customer.EmailAddress,
                DateOfBirth = customer.DateOfBirth
            };

            return customerResponse;
        }
        public List<CustomerResponseDTO> GenerateListCustomersResponseDTO(IQueryable<Customer> customers)
        {
            List<CustomerResponseDTO> customersResponses = new List<CustomerResponseDTO>();

            foreach (var customer in customers)
            {
                CustomerResponseDTO customerResponse = GenerateCustomerResponseDTO(customer: customer);
                customersResponses.Add(item: customerResponse);
            }

            return customersResponses;
        }

        public Customer GetCustomerByEmail(string emailAddress)
        {
            Customer customer = _repository.GetCustomerByEmail(emailAddress: emailAddress);
            return customer;
        }
    }
}