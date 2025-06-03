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