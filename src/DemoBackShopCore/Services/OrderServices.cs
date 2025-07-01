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
        private readonly ICustomerServices _customerServices;
        private readonly IProductRepository _productRepository;
        private readonly IProductServices _productServices;
        private readonly ApplicationDbContext _dbContext;

        public OrderServices
        (
            ApplicationDbContext dbContext,
            IOrderRepository repository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            ICustomerServices customerServices,
            IProductServices productServices
        )
        {
            _dbContext = dbContext;
            _repository = repository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _customerServices = customerServices;
            _productServices = productServices;
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

            if
            (
                customer.FirstName.ToLower()  != orderRequestDTO.Customer.FirstName.ToLower() 
                || customer.LastName.ToLower()  != orderRequestDTO.Customer.LastName.ToLower()
                || customer.DateOfBirth.ToDateTime(TimeOnly.MinValue).Date != orderRequestDTO.Customer.DateOfBirth.Date
            )
            {
                return ServiceResult<Order>.ErrorResult(message: DomainResponseMessages.CustomerOrderFieldsAreInvalidError, statusCode: 422); 
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

                if
                (
                    productExists.Code.ToLower() != item.Product.Code.ToLower()
                    ||
                    productExists.Name.ToLower() != item.Product.Name.ToLower()
                )
                {
                    return ServiceResult<Order>.ErrorResult
                    (
                        message: $"{DomainResponseMessages.ProductFieldsAreInvalidError}: Code:{item.Product.Code}", statusCode: 422
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

        public OrderResponseDTO GenerateOrderResponseDTO(Order order)
        {
            Customer? customer = _dbContext.Customers.AsNoTracking().Where(c => c.CustomerId == order.CustomerId).Include(c => c.Addresses).FirstOrDefault();

            CustomerResponseDTO customerResponseDTO = _customerServices.GenerateCustomerResponseDTO(customer: customer);

            List<ItemResponseDTO> items = new List<ItemResponseDTO>();

            foreach (var item in order.Items)
            {
                Product? product = _productServices.GetById(id: item.ProductId);

                ProductResponseDTO productResponseDTO = _productServices.GenerateProductResponseDTO(product: product);

                ItemResponseDTO itemResponseDTO = new ItemResponseDTO
                {
                    itemId = item.ItemId,
                    orderId = item.OrderId,
                    quantityOfItems = item.QuantityOfItems,
                    unitValue = item.UnitValue,
                    product = productResponseDTO
                };

                items.Add(item: itemResponseDTO);
            }

            OrderResponseDTO orderResponseDTO = new OrderResponseDTO
            {
                orderId = order.OrderId,
                orderNumber = order.OrderNumber,
                orderDate = order.OrderDate.ToString("yyyy-MM-dd"),
                totalOrderValue = order.TotalOrderValue,
                customer = customerResponseDTO,
                items = items
            };

            return orderResponseDTO;
        }

        public IQueryable<Order> GetAll(PaginationFilter paginationFilter)
        {
            return _dbContext.Orders
                    .AsNoTracking()
                    .Include(o => o.Items)
                    .Include(o => o.Customer)
                    .OrderBy(o => o.OrderId)
                    .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                    .Take(paginationFilter.PageSize);
        }

        public Order GetById(int id)
        {
            Order? findOrder = _dbContext.Orders.AsNoTracking().FirstOrDefault(o => o.OrderId == id);
            
            if (findOrder == null)
            {
                return null!;
            }

            Order? order = _dbContext.Orders.Where(o => o.OrderId == id).Include(o => o.Items).First();


            return order;
        }

        public ServiceResult<Order> Update(int id, OrderRequestDTO orderRequestDTO)
        {
            Order? order = _dbContext.Orders.AsNoTracking().Where(o => o.OrderId == id).Include(o => o.Items).FirstOrDefault();

            if (order == null)
            {
                return ServiceResult<Order>
                .ErrorResult
                (
                    message: DomainResponseMessages.OrderNotFoundMessageError,
                    statusCode: 404
                );
            }

            Customer? customer = _customerServices.GetCustomerByEmail(emailAddress: orderRequestDTO.Customer.EmailAddress);

            if (customer == null)
            {
                return ServiceResult<Order>.ErrorResult(message: DomainResponseMessages.CustomerNotFoundMessageError, statusCode: 404);
            }

            if
            (
                customer.FirstName.ToLower() != orderRequestDTO.Customer.FirstName.ToLower()
                || customer.LastName.ToLower() != orderRequestDTO.Customer.LastName.ToLower()
                || customer.DateOfBirth.ToDateTime(TimeOnly.MinValue).Date != orderRequestDTO.Customer.DateOfBirth.Date
            )
            {
                return ServiceResult<Order>.ErrorResult(message: DomainResponseMessages.CustomerOrderFieldsAreInvalidError, statusCode: 422);
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

                if
                (
                    productExists.Code.ToLower() != item.Product.Code.ToLower()
                    ||
                    productExists.Name.ToLower() != item.Product.Name.ToLower()
                )
                {
                    return ServiceResult<Order>.ErrorResult
                    (
                        message: $"{DomainResponseMessages.ProductFieldsAreInvalidError}: Code:{item.Product.Code}", statusCode: 422
                    );
                }

                Item newItem = Item.RegisterNew(product: productExists!, unitValue: item.UnitValue, quantityOfItems: item.QuantityOfItems);

                if (!newItem.IsValid)
                {
                    return ServiceResult<Order>.ErrorResult(message: newItem.ErrorMessageIfIsNotValid, statusCode: 422);
                }

                items.Add(item: newItem);
            }
            
            Order updatedOrder = Order.RegisterNew
            (
                orderNumber: orderRequestDTO.OrderNumber,
                orderDate: orderRequestDTO.OrderDate,
                customerId: customer.CustomerId,
                items: items
            );

            if (!updatedOrder.IsValid)
            {
                return ServiceResult<Order>.ErrorResult(message: updatedOrder.ErrorMessageIfIsNotValid, statusCode: 422);
            }

            _repository.Update(id: id, entity: updatedOrder);


            return ServiceResult<Order>.SuccessResult(data: updatedOrder);
        }
    }
}