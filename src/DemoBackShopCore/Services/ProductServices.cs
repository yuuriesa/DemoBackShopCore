using DemoBackShopCore.Data;
using DemoBackShopCore.DTOs;
using DemoBackShopCore.Models;
using DemoBackShopCore.Repository;
using DemoBackShopCore.Utils;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ServiceResult<Batch2ResponseResult>> AddBatch2(IEnumerable<ProductRequestDTO> productsRequestsDTO)
        {
            Batch2ResponseResult batch2ResponseResult = new Batch2ResponseResult();
            List<ProductRequestDTO> successTemporary = new List<ProductRequestDTO>();
            List<ProductRequestDTO> success = new List<ProductRequestDTO>();
            List<ProductDTOWithMessageErrors> failure = new List<ProductDTOWithMessageErrors>();

            IEnumerable<string> duplicateCodes = VerifyIfDuplicateCodes(productsRequestsDTO: productsRequestsDTO);

            if (duplicateCodes.Any())
            {
                foreach (var product in productsRequestsDTO)
                {
                    if (duplicateCodes.Contains(value: product.Code))
                    {
                        ProductDTOWithMessageErrors productDTOWithMessageErrors = new ProductDTOWithMessageErrors();
                        productDTOWithMessageErrors.Product = product;
                        productDTOWithMessageErrors.FailureErrorsMessages.Add(item: DomainResponseMessages.DuplicateCodeError);

                        failure.Add(item: productDTOWithMessageErrors);
                        continue;
                    }
                    successTemporary.Add(item: product);
                }
            }
            else
            {
                successTemporary.AddRange(collection: productsRequestsDTO);
            }

            List<Product> newProducts = new List<Product>();

            foreach (var product in successTemporary)
            {
                Product productExists = _repository.GetByCode(code: product.Code);

                if (productExists != null)
                {
                    ProductDTOWithMessageErrors productDTOWithMessageErrors = new ProductDTOWithMessageErrors();
                    productDTOWithMessageErrors.Product = product;
                    productDTOWithMessageErrors.FailureErrorsMessages.Add(item: $"{DomainResponseMessages.ProductCodeExistsError}: {product.Code}");

                    failure.Add(item: productDTOWithMessageErrors);
                    continue;
                }

                Product newProduct = Product.RegisterNew(code: product.Code, name: product.Name);

                if (!newProduct.IsValid)
                {
                    ProductDTOWithMessageErrors productDTOWithMessageErrors = new ProductDTOWithMessageErrors();
                    productDTOWithMessageErrors.Product = product;
                    productDTOWithMessageErrors.FailureErrorsMessages.Add(item: newProduct.ErrorMessageIfIsNotValid);

                    failure.Add(item: productDTOWithMessageErrors);
                    continue;
                }

                newProducts.Add(item: newProduct);
                success.Add(item: product);
            }

            _repository.AddRange(entities: newProducts);
            batch2ResponseResult.SuccessCount = success.Count;
            batch2ResponseResult.FailureCount = failure.Count;

            if (success.Count == 0)
            {
                batch2ResponseResult.Success = null;
            }
            else
            {
                batch2ResponseResult.Success.AddRange(success);
            }

            if (failure.Count == 0)
            {
                batch2ResponseResult.Failure = null;
            }
            else
            {
                batch2ResponseResult.Failure.AddRange(failure);
            }

            return ServiceResult<Batch2ResponseResult>.SuccessResult(data: batch2ResponseResult);
        }

        public Batch2PreparedForReponse GenerateBatch2PreparedResponseResult(Batch2ResponseResult batch2ResponseResult)
        {
            Batch2PreparedForReponse batch2PreparedForReponse = new Batch2PreparedForReponse();
            List<ProductResponseDTO> listSuccessProductsResponseDTOs = new List<ProductResponseDTO>();

            batch2PreparedForReponse.SuccessCount = batch2ResponseResult.SuccessCount;
            batch2PreparedForReponse.FailureCount = batch2ResponseResult.FailureCount;

            if (batch2ResponseResult.Success != null)
            {
                foreach (var successCustomer in batch2ResponseResult.Success)
                {
                    Product product = GetByCode(code: successCustomer.Code);

                    listSuccessProductsResponseDTOs.Add(item: GenerateProductResponseDTO(product: product));
                }
            }

            if (listSuccessProductsResponseDTOs.Count > 0)
            {
                batch2PreparedForReponse.Success = listSuccessProductsResponseDTOs;
            }
            else
            {
                batch2PreparedForReponse.Success = null;
            }

            batch2PreparedForReponse.Failure = batch2ResponseResult.Failure;

            return batch2PreparedForReponse;
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

        public ServiceResult<Product> Update(int id, ProductRequestDTO productRequestDTOforUpdate)
        {
            Product? getProductById = _dbContext
            .Products
            .AsNoTracking()
            .FirstOrDefault(p => p.ProductId == id);

            if (getProductById == null)
            {
                return ServiceResult<Product>.ErrorResult
                (
                    message: DomainResponseMessages.ProductNotFoundMessageError,
                    statusCode: 404
                );
            }

            Product codeProductExists = GetByCode(code: productRequestDTOforUpdate.Code);

            if (codeProductExists != null && codeProductExists.ProductId != id)
            {
                return ServiceResult<Product>.ErrorResult
                (
                    message: DomainResponseMessages.ProductCodeExistsError,
                    statusCode: 409
                );
            }

            Product updatedProduct = Product.SetExistingInfo
            (
                productId: getProductById.ProductId,
                code: productRequestDTOforUpdate.Code,
                name: productRequestDTOforUpdate.Name
            );

            if (!updatedProduct.IsValid)
            {
                return ServiceResult<Product>.ErrorResult
                (
                    message: updatedProduct.ErrorMessageIfIsNotValid,
                    statusCode: 422
                );
            }

            _repository.Update(id: id, entity: updatedProduct);

            return ServiceResult<Product>.SuccessResult(data: updatedProduct);
        }

        public IEnumerable<string> VerifyIfDuplicateCodes(IEnumerable<ProductRequestDTO> productsRequestsDTO)
        {
            IEnumerable<string> duplicateCodes = productsRequestsDTO.GroupBy(c => c.Code).Where(c => c.Count() > 1).Select(c => c.Key);

            return duplicateCodes;
        }
    }
}