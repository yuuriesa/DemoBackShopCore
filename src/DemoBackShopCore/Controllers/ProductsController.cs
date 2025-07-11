using DemoBackShopCore.Data;
using DemoBackShopCore.DTOs;
using DemoBackShopCore.Models;
using DemoBackShopCore.Services;
using DemoBackShopCore.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoBackShopCore.Controllers
{
    [ApiController]
    [Route("api/Product")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _services;
        private readonly ApplicationDbContext _dbContext;

        public ProductsController(IProductServices services, ApplicationDbContext dbContext)
        {
            _services = services;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 0 || pageSize < 0) return BadRequest(DomainResponseMessages.CustomerPaginationError);

            PaginationFilter paginationFilter = new PaginationFilter(pageNumber: pageNumber, pageSize: pageSize);

            IQueryable<Product> products = _services.GetAll(paginationFilter: paginationFilter);

            if (products.Count() == 0) return NoContent();

            List<ProductResponseDTO> listProductsResponses = _services.GenerateListProductResponseDTO(products: products);

            return Ok(listProductsResponses);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Product? product = _services.GetById(id: id);

            if (product == null)
            {
                return NotFound(DomainResponseMessages.ProductNotFoundMessageError);
            }

            ProductResponseDTO productResponse = _services.GenerateProductResponseDTO(product: product);

            return Ok(productResponse);
        }

        [HttpPost]
        public IActionResult Add(ProductRequestDTO productRequestDTO)
        {
            ServiceResult<Product>? result = _services.Add(productRequestDTO: productRequestDTO);

            if (!result.Success)
            {
                return StatusCode(statusCode: result.StatusCode, value: result.Message);
            }

            _dbContext.SaveChanges();

            ProductResponseDTO productResponse = _services.GenerateProductResponseDTO(product: result.Data);

            return CreatedAtAction(actionName: nameof(GetById), routeValues: new { id = result.Data.ProductId }, value: productResponse);
        }

        [HttpPost("batch")]
        public async Task<IActionResult> AddBatch(IEnumerable<ProductRequestDTO> productRequests)
        {
            //adicionar todos os produtos da lista, apenas se todos estiverem OK
            if (productRequests.Count() == 0) return NoContent();

            var transaction = await _dbContext.Database.BeginTransactionAsync();
            List<ProductResponseDTO> productsResponsesDTOs = new List<ProductResponseDTO>();

            try
            {
                ServiceResult<List<Product>> result = await _services.AddBatch(productsRequestsDTO: productRequests);

                if (!result.Success)
                {
                    return StatusCode(statusCode: result.StatusCode, value: result.Message);
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                foreach (var item in result.Data)
                {
                    productsResponsesDTOs.Add(item: _services.GenerateProductResponseDTO(product: item));
                }
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }


            return Ok(productsResponsesDTOs);
        }

        [HttpPost("batch2")]
        public async Task<IActionResult> AddBatch2(IEnumerable<ProductRequestDTO> productRequests)
        {
            if (productRequests.Count() == 0) return NoContent();

            var transaction = await _dbContext.Database.BeginTransactionAsync();
            Batch2PreparedForReponse responseResult = new Batch2PreparedForReponse();

            try
            {
                ServiceResult<Batch2ResponseResult>? result = await _services.AddBatch2(productsRequestsDTO: productRequests);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                responseResult = _services.GenerateBatch2PreparedResponseResult(batch2ResponseResult: result.Data);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

            return Ok(responseResult);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, ProductRequestDTO productRequestDTOforUpdate)
        {
            ServiceResult<Product> result = _services.Update(id: id, productRequestDTOforUpdate: productRequestDTOforUpdate);

            if (!result.Success)
            {
                return StatusCode(statusCode: result.StatusCode, value: result.Message);
            }

            _dbContext.SaveChanges();

            ProductResponseDTO productResponse = _services.GenerateProductResponseDTO(product: result.Data);

            return Ok(productResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            ServiceResult<Product> result = _services.Delete(id: id);

            if (!result.Success) return StatusCode(statusCode: result.StatusCode, value: result.Message);

            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}