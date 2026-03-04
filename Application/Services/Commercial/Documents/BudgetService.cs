using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Data;

namespace Application.Services.Commercial.Documents
{
    public abstract class BudgetService : CrudService<TEntity, TEntitySaveDto>
    {
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository repository, ICustomerQuery query, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, query, unitOfWork)
        {
            _mapper = mapper;
        }

        protected override Customer CreateEntity(CustomerSaveDto dto)
        {
            return new Customer(dto.Name, dto.PersonType, dto.Document, dto.Email, dto.StateRegistration, _mapper.Map<IEnumerable<CustomerAddress>>(dto.Addresses));
        }

        protected override void UpdateEntity(CustomerSaveDto dto, Customer entity)
        {
            entity.Fill(dto.Name, dto.PersonType, dto.Document, dto.Email, dto.StateRegistration, _mapper.Map<IEnumerable<CustomerAddress>>(dto.Addresses));
        }

        protected override Task<bool> EntitiesCanBeDeleted(IEnumerable<long> ids)
        {
            return Task.FromResult(true);
        }
    }
}