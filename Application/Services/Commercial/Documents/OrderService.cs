using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.Interfaces.Queries.Commercial.Documents.Orders;
using Application.Interfaces.Services.Commercial.Documents;
using AutoMapper;
using Domain.Entities.Commercial.Documents.Orders;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;
using Domain.ValueObjects.Commercial.Documents.Payments;

namespace Application.Services.Commercial.Documents
{
    public class OrderService : DocumentService<Order, OrderSaveDto, OrderProductSaveDto>, IOrderService
    {
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository repository, IOrderQuery query, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, query, unitOfWork)
        {
            _mapper = mapper;
        }

        protected override Order CreateEntity(OrderSaveDto dto)
        {
            List<OrderProduct> products = _mapper.Map<List<OrderProduct>>(dto.Products);

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
