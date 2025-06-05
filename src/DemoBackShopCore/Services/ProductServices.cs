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

        public async Task<ServiceResult<List<Product>>> AddBatch(IEnumerable<ProductRequestDTO> productsRequestsDTO)
        {
            IEnumerable<string> duplicateCodes = VerifyIfDuplicateCodes(productsRequestsDTO: productsRequestsDTO);

            if (duplicateCodes.Any())
            {
                return ServiceResult<List<Product>>.ErrorResult
                (
                    message: DomainResponseMessages.DuplicateCodeError,
                    statusCode: 400
                );
            }

            List<Product> products = new List<Product>();

            foreach (var product in productsRequestsDTO)
            {
                Product productExists = _repository.GetByCode(code: product.Code);

                if (productExists != null)
                {
                    return ServiceResult<List<Product>>.ErrorResult(message: $"{DomainResponseMessages.ProductCodeExistsError}: {product.Code}", statusCode: 409);
                }

                Product newProduct = Product.RegisterNew(code: product.Code, name: product.Name);

                if (!newProduct.IsValid)
                {
                    return ServiceResult<List<Product>>.ErrorResult(message: newProduct.ErrorMessageIfIsNotValid, statusCode: 422);
                }

                products.Add(item: newProduct);
            }

            _repository.AddRange(entities: products);

            return ServiceResult<List<Product>>.SuccessResult(data: products);
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

        public IEnumerable<string> VerifyIfDuplicateCodes(IEnumerable<ProductRequestDTO> productsRequestsDTO)
        {
            IEnumerable<string> duplicateCodes = productsRequestsDTO.GroupBy(c => c.Code).Where(c => c.Count() > 1).Select(c => c.Key);

            return duplicateCodes;
        }
    }
}