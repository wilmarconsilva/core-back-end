using Application.Authentication.Interfaces;
using Application.Documents.Orders.DataTransferObjects;
using Application.Documents.Orders.DataTransferObjects.Listings;
using Application.Documents.Orders.Queries;
using Application.Documents.Orders.Services.Interfaces;
using Application.Documents.Services;
using AutoMapper;
using Domain.Documents.Entities;
using Domain.Documents.Orders.Entities;
using Domain.Documents.Orders.Repositories.Interfaces;
using Domain.Documents.Orders.ValueObjects;
using Domain.Persons.Customers.Repositories.Interfaces;
using Domain.Shared.Persistance.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.Documents.Orders.Services
{
    public class OrderService : DocumentService<Order, OrderSaveDto, OrderDetailDto, OrderProductSaveDto, OrderListing, OrderListingFilter>, IOrderService
    {
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository repository,
            IOrderQuery query,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICustomerRepository customerRepository,
            IServiceScopeFactory serviceScopeFactory,
            IHttpContextCurrentUser currentUser,
            ILogger<OrderService> logger)
            : base(
                  repository,
                  query,
                  unitOfWork,
                  customerRepository,
                  serviceScopeFactory,
                  new("Pedido", "pedido"),
                  currentUser,
                  logger)
        {
            _mapper = mapper;
        }

        protected override IEnumerable<DocumentProduct> GetProducts(Order entity) => entity.Products;

        protected override Order CreateEntity(OrderSaveDto dto)
        {
            IReadOnlyCollection<OrderProduct> products = _mapper.Map<IReadOnlyCollection<OrderProduct>>(dto.Products);

            OrderPayment payment = CreateOrderPayment(dto);

            return new Order(_currentUser.UserId, dto.CreationDateTime, dto.CustomerId, payment, dto.Observations, dto.CustomerSignatureImage, dto.PDF, products, dto.BudgetId);
        }

        protected override void UpdateEntity(OrderSaveDto dto, Order entity)
        {
            List<OrderProduct> products = _mapper.Map<List<OrderProduct>>(dto.Products);

            OrderPayment payment = CreateOrderPayment(dto);

            entity.Fill(_currentUser.UserId, dto.CustomerId, payment, dto.Observations, dto.CustomerSignatureImage, dto.PDF, products);
        }

        protected override Task<bool> EntitiesCanBeDeleted(IEnumerable<long> ids)
        {
            return Task.FromResult(true);
        }

        private static OrderPayment CreateOrderPayment(OrderSaveDto dto)
        {
            return new OrderPayment(dto.Payment.Method, dto.Payment.DiscountValue, CreateInstallment(dto.Payment.Installment));
        }
    }
}
