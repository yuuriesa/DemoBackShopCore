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
                                    dateOfBirth: customerRequest.DateOfBirth,
                                    addresses: customerRequest.Addresses
                                );
            
            if (!newCustomer.IsValid())
            {
                return ServiceResult<Customer>.ErrorResult(message: newCustomer.ErrorMessageIfIsNotValid, statusCode: 422);
            }

            _repository.Add(entity: newCustomer);
            return ServiceResult<Customer>.SuccessResult(data: newCustomer, statusCode: 201);
        }

        public async Task<ServiceResult<List<Customer>>> AddBatch(IEnumerable<CustomerRequestDTO> customerRequests)
        {
            IEnumerable<string> duplicateEmails = customerRequests.GroupBy(c => c.EmailAddress).Where(c => c.Count() > 1).Select(c => c.Key);

            if (duplicateEmails.Any())
            {
                return ServiceResult<List<Customer>>.ErrorResult
                (
                    message: DomainResponseMessages.DuplicateEmailError,
                    statusCode: 400
                );
            }

            List<Customer> newListCustomers = new List<Customer>();

            foreach (var customer in customerRequests)
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
                    dateOfBirth: customer.DateOfBirth,
                    addresses: customer.Addresses
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

        public async Task<ServiceResult<Batch2CustomerResponseResult>> AddBatch2(IEnumerable<CustomerRequestDTO> customerRequests)
        {
            Batch2CustomerResponseResult batch2CustomerResponseResult = new Batch2CustomerResponseResult();
            List<CustomerRequestDTO> successTemporary = new List<CustomerRequestDTO>();
            List<CustomerRequestDTO> success = new List<CustomerRequestDTO>();
            List<CustomerDTOWithMessageErrors> failure = new List<CustomerDTOWithMessageErrors>();

            IEnumerable<string> duplicateEmails = customerRequests.GroupBy(c => c.EmailAddress).Where(c => c.Count() > 1).Select(c => c.Key);

            //Ele adiciona no failure certinho. Vi pelo Debug, então pode pular.
            if (duplicateEmails.Any())
            {
                foreach (var customer in customerRequests)
                {
                    CustomerDTOWithMessageErrors customerDTOWithMessageErrors = new CustomerDTOWithMessageErrors(); 
                    if (duplicateEmails.Contains(value: customer.EmailAddress))
                    {
                        customerDTOWithMessageErrors.Customer = customer;
                        customerDTOWithMessageErrors.FailureErrorsMessages.Add(item: DomainResponseMessages.DuplicateEmailError);
                        failure.Add(item: customerDTOWithMessageErrors);
                        continue;
                    }
                    successTemporary.Add(item: customer);
                }
            }

            // Sobre essa validação, eu notei que eu estou refazendo o loop em toda a lista de customers
            // Ou seja, se antes já tinha customers no failure, eu estou passando por eles denovo, o que eu não posso
            // Tenho que começar esse loop ignorando os que já estão no failure
            // Então eu preciso fazer uma lista de failure e success temporarias.

            List<Customer> newListCustomers = new List<Customer>();

            foreach (var customer in successTemporary)
            {
                Customer customerExists = GetCustomerByEmail(emailAddress: customer.EmailAddress);

                if (customerExists != null)
                {
                    CustomerDTOWithMessageErrors customerDTOWithMessageErrors = new CustomerDTOWithMessageErrors();
                    
                    customerDTOWithMessageErrors.Customer = customer;
                    customerDTOWithMessageErrors.FailureErrorsMessages.Add(item: $"{DomainResponseMessages.CustomerEmailExistsError}: {customerExists.EmailAddress}");
                    failure.Add(item: customerDTOWithMessageErrors);
                    continue;
                }
                
                Customer newCustomer = Customer.RegisterNew
                (
                    firstName: customer.FirstName,
                    lastName: customer.LastName,
                    emailAddress: customer.EmailAddress,
                    dateOfBirth: customer.DateOfBirth,
                    addresses: customer.Addresses
                );

                if (!newCustomer.IsValid())
                {
                    CustomerDTOWithMessageErrors customerDTOWithMessageErrors = new CustomerDTOWithMessageErrors();
                    
                    customerDTOWithMessageErrors.Customer = customer;
                    customerDTOWithMessageErrors.FailureErrorsMessages.Add(item: $"{newCustomer.ErrorMessageIfIsNotValid} - customer: {newCustomer.EmailAddress}");
                    failure.Add(item: customerDTOWithMessageErrors);
                    continue;
                }

                newListCustomers.Add(item: newCustomer);
                success.Add(item: customer);
            }

            _repository.AddRange(entities: newListCustomers);
            batch2CustomerResponseResult.CustomersSuccessCount = success.Count;
            batch2CustomerResponseResult.CustomersFailureCount = failure.Count;

            return ServiceResult<Batch2CustomerResponseResult>.SuccessResult(data: batch2CustomerResponseResult);
        }

        public IQueryable<Customer> GetAll(PaginationFilter paginationFilter)
        {
            return _repository.GetAll(paginationFilter: paginationFilter).Include(a => a.Addresses);
        }

        public Customer GetById(int id)
        {
            Customer customer = _dbContext.Customers.Where(c => c.CustomerId == id).Include(c => c.Addresses).FirstOrDefault();
            if (customer == null)
            {
                return null!;
            }

            return customer;
        }

        public ServiceResult<Customer> Remove(int id)
        {
            Customer customer = GetById(id: id);
            if (customer == null)
            {
                return ServiceResult<Customer>.ErrorResult(message: DomainResponseMessages.CustomerNotFoundMessageError, statusCode: 404);
            }

            _dbContext.Addresses.RemoveRange(entities: customer.Addresses);

            _repository.Remove(id: id);
            return ServiceResult<Customer>.SuccessResult(data: customer);
        }

        public ServiceResult<Customer> Update(int id, CustomerRequestDTO customerRequestDTO)
        {
            Customer customerExists = _dbContext.Customers.AsNoTracking().Where(c => c.CustomerId == id).Include(c => c.Addresses).FirstOrDefault();

            if (customerExists == null)
            {
                return ServiceResult<Customer>.ErrorResult
                    (
                        message: DomainResponseMessages.CustomerNotFoundMessageError,
                        statusCode: 404
                    ); 
            }

            Customer getCustomerByEmail = _dbContext.Customers.AsNoTracking().FirstOrDefault(c => c.EmailAddress == customerRequestDTO.EmailAddress);

            if (getCustomerByEmail != null && getCustomerByEmail.CustomerId != id)
            {
                return ServiceResult<Customer>.ErrorResult
                    (
                        message: DomainResponseMessages.CustomerEmailExistsError,
                        statusCode: 409
                    ); 
            }

            List<Address> newAddresses = new List<Address>();

            foreach (var address in customerRequestDTO.Addresses)
            {
                Address newAddress = new Address
                {
                    ZipCode = address.ZipCode,
                    Street = address.Street,
                    Number = address.Number,
                    Neighborhood = address.Neighborhood,
                    AddressComplement = address.AddressComplement,
                    City = address.City,
                    State = address.State,
                    Country = address.Country
                };

                newAddresses.Add(item: newAddress);
            }

            _dbContext.Addresses.RemoveRange(entities: customerExists.Addresses);
            _dbContext.SaveChanges();

            Customer updatedCustomer = Customer.SetExistingInfo
            (
                customerId: customerExists.CustomerId,
                firstName: customerRequestDTO.FirstName,
                lastName: customerRequestDTO.LastName,
                emailAddress: customerRequestDTO.EmailAddress,
                dateOfBirth: DateOnly.FromDateTime(customerRequestDTO.DateOfBirth),
                addresses: newAddresses
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
            List<AddressResponseDTO> addresses = new List<AddressResponseDTO>();

            foreach (var address in customer.Addresses)
            {
                addresses.Add(item: new AddressResponseDTO
                {
                    AddressId = address.AddressId,
                    ZipCode = address.ZipCode,
                    Street = address.Street,
                    Number = address.Number,
                    Neighborhood = address.Neighborhood,
                    AddressComplement = address.AddressComplement,
                    City = address.City,
                    State = address.State,
                    Country = address.Country
                });
            }

            CustomerResponseDTO customerResponse = new CustomerResponseDTO
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                EmailAddress = customer.EmailAddress,
                DateOfBirth = customer.DateOfBirth,
                Addresses = addresses
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