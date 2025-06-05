using DemoBackShopCore.DTOs;
using DemoBackShopCore.Models;
using DemoBackShopCore.Utils;

namespace DemoBackShopCore.Services
{
    public interface IProductServices
    {
        public IQueryable<Product> GetAll(PaginationFilter paginationFilter);
        public Product GetById(int id);
        public Product GetByCode(string code);
        public ServiceResult<Product> Add(ProductRequestDTO productRequestDTO);
        public Task<ServiceResult<List<Product>>> AddBatch(IEnumerable<ProductRequestDTO> productsRequestsDTO);
        public ProductResponseDTO GenerateProductResponseDTO(Product product);
        public List<ProductResponseDTO> GenerateListProductResponseDTO(IQueryable<Product> products);
    }
}