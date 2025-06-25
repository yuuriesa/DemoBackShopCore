using DemoBackShopCore.Data;
using DemoBackShopCore.DTOs;
using DemoBackShopCore.Models;
using DemoBackShopCore.Repository;
using DemoBackShopCore.Utils;
using Microsoft.EntityFrameworkCore;

namespace DemoBackShopCore.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _repository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _dbContext;

        public OrderServices
        (
            ApplicationDbContext dbContext,
            IOrderRepository repository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository
        )
        {
            _dbContext = dbContext;
            _repository = repository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;

        }

        public ServiceResult<Order> Add(OrderRequestDTO orderRequestDTO)
        {
            Order? orderNumberExists = _dbContext.Orders.ToList().FirstOrDefault(o => o.OrderNumber == orderRequestDTO.OrderNumber);

            if (orderNumberExists != null)
            {
                return ServiceResult<Order>.ErrorResult(message: $"{DomainResponseMessages.OrderNumberExistsError}: {orderRequestDTO.OrderNumber}", statusCode: 409);
            }

            Customer customer = _customerRepository.GetCustomerByEmail(emailAddress: orderRequestDTO.Customer.EmailAddress);

            if (customer == null)
            {
                return ServiceResult<Order>.ErrorResult(message: DomainResponseMessages.CustomerNotFoundMessageError, statusCode: 404);
            }
            
            List<Item> items = new List<Item>();

            foreach (var item in orderRequestDTO.Items)
            {
                Product? productExists = _productRepository.GetByCode(code: item.Product.Code);

                if (productExists == null)
                {
                    return ServiceResult<Order>.ErrorResult
                    (
                        message: $"{DomainResponseMessages.ProductNotFoundMessageError}: Code:{item.Product.Code}", statusCode: 404
                    );
                }

                Item newItem = Item.RegisterNew(product: productExists!, unitValue: item.UnitValue, quantityOfItems: item.QuantityOfItems);

                if (!newItem.IsValid)
                {
                    return ServiceResult<Order>.ErrorResult(message: newItem.ErrorMessageIfIsNotValid, statusCode: 422);
                }

                items.Add(item: newItem);
            }


            Order newOrder = Order.RegisterNew
            (
                orderNumber: orderRequestDTO.OrderNumber,
                orderDate: orderRequestDTO.OrderDate,
                customerId: customer.CustomerId,
                items: items
            );

            if (!newOrder.IsValid)
            {
                return ServiceResult<Order>.ErrorResult(message: newOrder.ErrorMessageIfIsNotValid, statusCode: 422);
            }

            _repository.Add(entity: newOrder);


            return ServiceResult<Order>.SuccessResult(data: newOrder, 201);
        }

        public IQueryable<Order> GetAll(PaginationFilter paginationFilter)
        {
            return _repository.GetAll(paginationFilter: paginationFilter);
        }
    }
}