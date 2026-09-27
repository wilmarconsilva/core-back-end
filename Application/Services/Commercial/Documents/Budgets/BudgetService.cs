using Application.DataTransferObjects.Commercial.Documents;
using Application.DataTransferObjects.Commercial.Documents.Budgets;
using Application.DataTransferObjects.Commercial.Documents.Budgets.Listings;
using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.Interfaces.Queries.Commercial.Documents.Budgets;
using Application.Interfaces.Security;
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
using Microsoft.Extensions.Logging;

namespace Application.Services.Commercial.Documents.Budgets
{
    public class BudgetService : DocumentService<Budget, BudgetSaveDto, DocumentDetailDto, BudgetProductSaveDto, BudgetListing, BudgetListingFilter>, IBudgetService
    {
        private readonly IMapper _mapper;

        public BudgetService(IBudgetRepository repository,
            IBudgetQuery query,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICustomerRepository customerRepository,
            IServiceScopeFactory serviceScopeFactory,
            IHttpContextCurrentUser currentUser,
            ILogger<BudgetService> logger)
            : base(repository,
                  query,
                  unitOfWork,
                  customerRepository,
                  serviceScopeFactory,
                  new("Orçamento", "orcamento"),
                  currentUser,
                  logger)
        {
            _mapper = mapper;
        }

        protected override IEnumerable<DocumentProduct> GetProducts(Budget entity) => entity.Products;

        protected override Budget CreateEntity(BudgetSaveDto dto)
        {
            IReadOnlyCollection<BudgetProduct> products = _mapper.Map<IReadOnlyCollection<BudgetProduct>>(dto.Products);

            Payment payment = CreatePayment(dto);

            return new Budget(_currentUser.UserId, dto.CreationDateTime, dto.CustomerId, payment, dto.Observations, dto.CustomerSignatureImage, dto.PDF, products);
        }

        protected override void UpdateEntity(BudgetSaveDto dto, Budget entity)
        {
            IReadOnlyCollection<BudgetProduct> products = _mapper.Map<IReadOnlyCollection<BudgetProduct>>(dto.Products);
            Payment payment = CreatePayment(dto);

            entity.Fill(_currentUser.UserId, dto.CustomerId, payment, dto.Observations, dto.CustomerSignatureImage, dto.PDF, products);
        }

        protected override Task<bool> EntitiesCanBeDeleted(IEnumerable<long> ids)
        {
            return Task.FromResult(true);
        }
    }
}
