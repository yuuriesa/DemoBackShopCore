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
        public Task<ServiceResult<List<Customer>>> AddBatch(IEnumerable<CustomerRequestDTO> customerRequests);
        public Task<ServiceResult<Batch2CustomerResponseResult>> AddBatch2(IEnumerable<CustomerRequestDTO> customerRequests);
        public ServiceResult<Customer> Update(int id, CustomerRequestDTO customerRequestDTO);
        public ServiceResult<Customer> Remove(int id);
        public CustomerResponseDTO GenerateCustomerResponseDTO(Customer customer);
        public List<CustomerResponseDTO> GenerateListCustomersResponseDTO(IQueryable<Customer> customers);
        public Batch2PreparedForCustomerReponse GenerateBatch2PreparedCustomerResponseResult(Batch2CustomerResponseResult batch2CustomerResponseResult);
    }
}