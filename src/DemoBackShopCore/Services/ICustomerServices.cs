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
        public Customer GetCustomerByEmail(string emailAddress);
        public ServiceResult<Customer> Add(CustomerRequestDTO customerRequest);
        public Task<ServiceResult<List<Customer>>> AddBatch(IEnumerable<CustomerRequestDTO> customers);
        public ServiceResult<Customer> Update(int id, CustomerRequestDTO customerRequestDTO);
        public ServiceResult<Customer> Remove(int id);
        public CustomerResponseDTO GenerateCustomerResponseDTO(Customer customer);
        public List<CustomerResponseDTO> GenerateListCustomersResponseDTO(IQueryable<Customer> customers);
    }
}