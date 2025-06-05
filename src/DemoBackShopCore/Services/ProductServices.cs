using DemoBackShopCore.Data;
using DemoBackShopCore.DTOs;
using DemoBackShopCore.Models;
using DemoBackShopCore.Repository;
using DemoBackShopCore.Utils;

namespace DemoBackShopCore.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _repository;
        private readonly ApplicationDbContext _dbContext;

        public ProductServices
        (
            IProductRepository repository,
            ApplicationDbContext dbContext
        )
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public ServiceResult<Product> Add(ProductRequestDTO productRequestDTO)
        {
            Product productExists = _repository.GetByCode(code: productRequestDTO.Code);

            if (productExists != null)
            {
                return ServiceResult<Product>.ErrorResult(message: $"{DomainResponseMessages.ProductCodeExistsError}: {productRequestDTO.Code}", statusCode: 409);
            }

            Product newProduct = Product.RegisterNew(code: productRequestDTO.Code, name: productRequestDTO.Name);

            if (!newProduct.IsValid)
            {
                return ServiceResult<Product>.ErrorResult(message: newProduct.ErrorMessageIfIsNotValid, statusCode: 422);
            }

            _repository.Add(entity: newProduct);

            return ServiceResult<Product>.SuccessResult(data: newProduct, statusCode: 201);
        }

        public Task<ServiceResult<Product>> AddBatch(IEnumerable<ProductRequestDTO> productRequestDTO)
        {
            throw new NotImplementedException();
        }

        public List<ProductResponseDTO> GenerateListProductResponseDTO(IQueryable<Product> products)
        {
            List<ProductResponseDTO> listProductsResponses = new List<ProductResponseDTO>();

            foreach (var product in products)
            {
                Product getProduct = GetByCode(code: product.Code);

                ProductResponseDTO productResponse = new ProductResponseDTO
                {
                    ProductId = getProduct.ProductId,
                    Code = getProduct.Code,
                    Name = getProduct.Name
                };

                listProductsResponses.Add(item: productResponse);
            }

            return listProductsResponses;
        }

        public ProductResponseDTO GenerateProductResponseDTO(Product product)
        {
            Product getProduct = GetByCode(code: product.Code);

            ProductResponseDTO productResponse = new ProductResponseDTO
            {
                ProductId = getProduct.ProductId,
                Code = getProduct.Code,
                Name = getProduct.Name
            };

            return productResponse;
        }
        public IQueryable<Product> GetAll(PaginationFilter paginationFilter)
        {
            return _repository.GetAll(paginationFilter: paginationFilter);
        }

        public Product GetByCode(string code)
        {
            return _repository.GetByCode(code: code);
        }

        public Product GetById(int id)
        {
            return _repository.GetById(id: id);
        }
    }
}