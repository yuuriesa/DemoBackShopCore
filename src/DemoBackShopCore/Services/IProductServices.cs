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
        public Task<ServiceResult<Batch2ResponseResult>> AddBatch2(IEnumerable<ProductRequestDTO> productsRequestsDTO);
        public ServiceResult<Product> Update(int id, ProductRequestDTO productRequestDTOforUpdate);
        public ServiceResult<Product> Delete(int id);
        public IEnumerable<string> VerifyIfDuplicateCodes(IEnumerable<ProductRequestDTO> productsRequestsDTO);
        public ProductResponseDTO GenerateProductResponseDTO(Product product);
        public List<ProductResponseDTO> GenerateListProductResponseDTO(IQueryable<Product> products);
        public Batch2PreparedForReponse GenerateBatch2PreparedResponseResult(Batch2ResponseResult batch2ResponseResult);
    }
}