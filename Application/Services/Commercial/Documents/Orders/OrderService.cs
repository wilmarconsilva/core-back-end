using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.DataTransferObjects.Commercial.Documents.Orders.Listings;
using Application.DataTransferObjects.Entities;
using Application.Interfaces.Queries.Commercial.Documents.Orders;
using Application.Interfaces.Services.Commercial.Documents.Orders;
using AutoMapper;
using Domain.Entities.Commercial.Documents.Budgets;
using Domain.Entities.Commercial.Documents.Orders;
using Domain.Exceptions;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;
using Domain.ValueObjects.Commercial.Documents.Payments;

namespace Application.Services.Commercial.Documents.Orders
{
    public class OrderService : DocumentService<Order, OrderSaveDto, OrderProductSaveDto>, IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderQuery _orderQuery;

        public OrderService(IOrderRepository repository, IOrderQuery query, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, query, unitOfWork)
        {
            _orderQuery = query;
            _mapper = mapper;
        }

        public async Task<OrderSaveDto> CreateByBudgetAsync(Budget budget, CancellationToken cancellationToken)
        {
            Order order = new Order(budget);

            await SaveAsync([order], cancellationToken);

            return _mapper.Map<Order, OrderSaveDto>(order);
        }

        public override async Task<List<TListing>> GetAllAsync<TListing, TFilter>(TFilter filter, CancellationToken cancellationToken)
        {
            if (filter is not OrderListingFilter orderFilter || typeof(TListing) != typeof(OrderListing))
                throw new BusinessRuleException($"Listagem inválida para {nameof(OrderService)}.");

            List<OrderListing> orders = await _orderQuery.GetAll(orderFilter, cancellationToken);

            return orders.Cast<TListing>().ToList();
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
