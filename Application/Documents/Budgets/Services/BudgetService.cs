using Application.Authentication.Interfaces;
using Application.Documents.Budgets.DataTransferObjects;
using Application.Documents.Budgets.DataTransferObjects.Listings;
using Application.Documents.Budgets.Queries.Interfaces;
using Application.Documents.Budgets.Services.Interfaces;
using Application.Documents.Services;
using AutoMapper;
using Domain.Documents.Budgets.Entities;
using Domain.Documents.Budgets.Repositories.Interfaces;
using Domain.Documents.Entities;
using Domain.Documents.ValueObjects.Payments;
using Domain.Persons.Customers.Repositories.Interfaces;
using Domain.Shared.Persistance.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.Documents.Budgets.Services
{
    public class BudgetService : DocumentService<Budget, BudgetSaveDto, BudgetDetailDto, BudgetProductSaveDto, BudgetListing, BudgetListingFilter>, IBudgetService
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

            Payment payment = CreatePayment(dto.Payment);

            return new Budget(_currentUser.UserId, dto.CreationDateTime, dto.CustomerId, payment, dto.Observations, dto.CustomerSignatureImage, dto.PDF, products);
        }

        protected override void UpdateEntity(BudgetSaveDto dto, Budget entity)
        {
            IReadOnlyCollection<BudgetProduct> products = _mapper.Map<IReadOnlyCollection<BudgetProduct>>(dto.Products);

            Payment payment = CreatePayment(dto.Payment);

            entity.Fill(_currentUser.UserId, dto.CustomerId, payment, dto.Observations, dto.CustomerSignatureImage, dto.PDF, products);
        }

        protected override Task<bool> EntitiesCanBeDeleted(IEnumerable<long> ids)
        {
            return Task.FromResult(true);
        }
    }
}
