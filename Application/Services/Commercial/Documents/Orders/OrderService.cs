using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.DataTransferObjects.Commercial.Documents.Orders.Listings;
using Application.Interfaces.Queries.Commercial.Documents.Orders;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Commercial.Documents.Orders;
using AutoMapper;
using Domain.Entities.Commercial.Documents.Budgets;
using Domain.Entities.Commercial.Documents.Orders;
using Domain.Entities.Persons.Customers;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;
using Domain.ValueObjects.Commercial.Documents.Payments;

namespace Application.Services.Commercial.Documents.Orders
{
    public class OrderService : DocumentService<Order, OrderSaveDto, OrderProductSaveDto, OrderListing, OrderListingFilter>, IOrderService
    {
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository repository, IOrderQuery query, IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, ICustomerRepository customerRepository, IPdfService pdfService, ITemplateRenderer templateRenderer) : base(repository, query, unitOfWork, emailService, customerRepository, pdfService, templateRenderer)
        {
            _mapper = mapper;
        }

        protected override long GetCustomerId(Order entity) => entity.CustomerId;

        protected override string GetEmailSubject(Order entity) => "Pedido gerado";

        protected override string GetEmailBody(Order entity) => $"Seu pedido de código: {entity.Id} foi processado com sucesso!";

        protected override string GetDocumentFileName(Order entity) => $"pedido-{entity.Id}.pdf";

        protected override string GetTemplateName() => "order";

        protected override object GetTemplateModel(Order entity, Customer customer) => new
        {
            entity.Id,
            entity.BudgetId,
            entity.ValueSubtotal,
            entity.ValueTotal,
            entity.Observations,
            CreationDateTime = entity.CreationDateTime.ToString("dd/MM/yyyy HH:mm"),
            CustomerName = customer.Name,
            CustomerDocument = customer.Document,
        };

        public async Task<OrderSaveDto> CreateByBudgetAsync(Budget budget, CancellationToken cancellationToken)
        {
            Order order = new Order(budget);

            await SaveAsync([order], cancellationToken);

            return _mapper.Map<Order, OrderSaveDto>(order);
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
