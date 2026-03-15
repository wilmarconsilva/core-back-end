using Application.DataTransferObjects.Commercial.Documents.Budgets;
using Application.Interfaces.Queries.Commercial.Documents.Budgets;
using Application.Interfaces.Services.Commercial.Documents;
using AutoMapper;
using Domain.Entities.Commercial.Documents.Budgets;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;
using Domain.ValueObjects.Commercial.Documents.Payments;

namespace Application.Services.Commercial.Documents
{
    public class BudgetService : DocumentService<Budget, BudgetSaveDto, BudgetProductSaveDto>, IBudgetService
    {
        private readonly IMapper _mapper;

        public BudgetService(IBudgetRepository repository, IBudgetQuery query, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, query, unitOfWork)
        {
            _mapper = mapper;
        }

        protected override Budget CreateEntity(BudgetSaveDto dto)
        {
            List<BudgetProduct> products = _mapper.Map<List<BudgetProduct>>(dto.Products);

            Payment payment = CreatePayment(dto);

            return new Budget(dto.UserId, dto.CustomerId, payment, dto.Observations, dto.CustomerSignatureImage, products);
        }

        protected override void UpdateEntity(BudgetSaveDto dto, Budget entity)
        {
            List<BudgetProduct> products = _mapper.Map<List<BudgetProduct>>(dto.Products);
            Payment payment = CreatePayment(dto);

            entity.Fill(dto.UserId, dto.CustomerId, payment, dto.Observations, dto.CustomerSignatureImage, products);
        }

        protected override Task<bool> EntitiesCanBeDeleted(IEnumerable<long> ids)
        {
            return Task.FromResult(true);
        }
    }
}