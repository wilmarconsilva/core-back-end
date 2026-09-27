using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.DataTransferObjects.Commercial.Documents.Orders.Listings;
using Application.Interfaces.Queries.Commercial.Documents.Orders;
using Application.Interfaces.Security;
using Application.Interfaces.Services.Commercial.Documents.Orders;
using AutoMapper;
using Domain.Entities.Commercial.Documents;
using Domain.Entities.Commercial.Documents.Orders;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;
using Domain.ValueObjects.Commercial.Documents.Payments;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.Services.Commercial.Documents.Orders
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

            Payment payment = CreatePayment(dto);

            return new Order(_currentUser.UserId, dto.CreationDateTime, dto.CustomerId, payment, dto.Observations, dto.CustomerSignatureImage, dto.PDF, products, dto.BudgetId);
        }

        protected override void UpdateEntity(OrderSaveDto dto, Order entity)
        {
            List<OrderProduct> products = _mapper.Map<List<OrderProduct>>(dto.Products);
            Payment payment = CreatePayment(dto);

            entity.Fill(_currentUser.UserId, dto.CustomerId, payment, dto.Observations, dto.CustomerSignatureImage, dto.PDF, products);
        }

        protected override Task<bool> EntitiesCanBeDeleted(IEnumerable<long> ids)
        {
            return Task.FromResult(true);
        }
    }
}
