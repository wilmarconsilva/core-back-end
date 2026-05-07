using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.DataTransferObjects.Commercial.Documents.Orders.Listings;
using Application.Interfaces.Queries.Commercial.Documents.Orders;
using System;
using Application.Interfaces.Services.Commercial.Documents.Orders;
using AutoMapper;
using Domain.Entities.Commercial.Documents;
using Domain.Entities.Commercial.Documents.Budgets;
using Domain.Entities.Commercial.Documents.Orders;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;
using Domain.ValueObjects.Commercial.Documents.Payments;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Commercial.Documents.Orders
{
    public class OrderService : DocumentService<Order, OrderSaveDto, OrderDetailDto, OrderProductSaveDto, OrderListing, OrderListingFilter>, IOrderService
    {
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository repository, IOrderQuery query, IUnitOfWork unitOfWork, IMapper mapper, ICustomerRepository customerRepository, IProductRepository productRepository, IServiceScopeFactory serviceScopeFactory)
            : base(repository, query, unitOfWork, customerRepository, productRepository, serviceScopeFactory, new("Pedido", "pedido"))
        {
            _mapper = mapper;
        }

        protected override IEnumerable<DocumentProduct> GetProducts(Order entity) => entity.Products;

        protected override void AppendExtraTemplateFields(Order entity, Dictionary<string, object> model)
        {
            if (entity.BudgetId.HasValue)
                model["BudgetId"] = entity.BudgetId.Value;
        }

        public async Task<OrderDetailDto> CreateByBudgetAsync(Budget budget, CancellationToken cancellationToken)
        {
            Order order = new Order(budget);

            await SaveAsync([order], cancellationToken);

            OrderDetailDto? detail = await _query.GetDetailAsync(order.Id, cancellationToken);
            if (detail == null)
                throw new InvalidOperationException($"Pedido {order.Id} não encontrado após criação.");

            return detail;
        }

        protected override Order CreateEntity(OrderSaveDto dto)
        {
            IReadOnlyCollection<OrderProduct> products = _mapper.Map<IReadOnlyCollection<OrderProduct>>(dto.Products);

            Payment payment = CreatePayment(dto);

            return new Order(dto.UserId, dto.CustomerId, payment, dto.Observations, dto.CustomerSignatureImage, products, dto.BudgetId);
        }

        protected override void UpdateEntity(OrderSaveDto dto, Order entity)
        {
            List<OrderProduct> products = _mapper.Map<List<OrderProduct>>(dto.Products);
            Payment payment = CreatePayment(dto);

            entity.Fill(dto.UserId, dto.CustomerId, payment, dto.Observations, dto.CustomerSignatureImage, products);
        }

        protected override Task<bool> EntitiesCanBeDeleted(IEnumerable<long> ids)
        {
            return Task.FromResult(true);
        }
    }
}
