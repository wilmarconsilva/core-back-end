using Application.DataTransferObjects.Commercial.Documents.Budgets;
using Application.DataTransferObjects.Commercial.Documents.Budgets.Listings;
using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.Interfaces.Queries.Commercial.Documents.Budgets;
using Application.Interfaces.Services.Commercial.Documents;
using Application.Interfaces.Services.Commercial.Documents.Orders;
using AutoMapper;
using Domain.Entities.Commercial.Documents;
using Domain.Entities.Commercial.Documents.Budgets;
using Domain.Exceptions;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;
using Domain.ValueObjects.Commercial.Documents.Payments;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Commercial.Documents.Budgets
{
    public class BudgetService : DocumentService<Budget, BudgetSaveDto, BudgetProductSaveDto, BudgetListing, BudgetListingFilter>, IBudgetService
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public BudgetService(IBudgetRepository repository, IBudgetQuery query, IUnitOfWork unitOfWork, IMapper mapper, IOrderService orderService, ICustomerRepository customerRepository, IProductRepository productRepository, IServiceScopeFactory serviceScopeFactory)
            : base(repository, query, unitOfWork, customerRepository, productRepository, serviceScopeFactory, new("Orçamento", "orcamento"))
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        protected override IEnumerable<DocumentProduct> GetProducts(Budget entity) => entity.Products;

        protected override Budget CreateEntity(BudgetSaveDto dto)
        {
            IReadOnlyCollection<BudgetProduct> products = _mapper.Map<IReadOnlyCollection<BudgetProduct>>(dto.Products);

            Payment payment = CreatePayment(dto);

            return new Budget(dto.UserId, dto.CustomerId, payment, dto.Observations, dto.CustomerSignatureImage, products);
        }

        protected override void UpdateEntity(BudgetSaveDto dto, Budget entity)
        {
            IReadOnlyCollection<BudgetProduct> products = _mapper.Map<IReadOnlyCollection<BudgetProduct>>(dto.Products);
            Payment payment = CreatePayment(dto);

            entity.Fill(dto.UserId, dto.CustomerId, payment, dto.Observations, dto.CustomerSignatureImage, products);
        }

        public async Task<OrderSaveDto> CreateOrderByBudget(long id, CancellationToken cancellationToken)
        {
            Budget? budget = (await _repositoryCrud.GetByIdsAsync([id], cancellationToken)).FirstOrDefault();

            if (budget == null)
                throw new BusinessRuleException(GetNotFoundMessage([id]));

            OrderSaveDto orderSaveDto = await _orderService.CreateByBudgetAsync(budget, cancellationToken);

            return orderSaveDto;
        }

        protected override Task<bool> EntitiesCanBeDeleted(IEnumerable<long> ids)
        {
            return Task.FromResult(true);
        }
    }
}