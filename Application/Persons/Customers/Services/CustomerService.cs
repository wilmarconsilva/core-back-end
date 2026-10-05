using Application.Persons.Customers.DataTransferObjects;
using Application.Persons.Customers.DataTransferObjects.Listings;
using Application.Persons.Customers.DataTransferObjects.Synchronizations;
using Application.Persons.Customers.Queries;
using Application.Persons.Customers.Services.Interfaces;
using Application.Shared.Services;
using AutoMapper;
using Domain.Persons.Customers.Entities;
using Domain.Persons.Customers.Repositories.Interfaces;
using Domain.Shared.Persistance.Interfaces;

namespace Application.Persons.Customers.Services
{
    public class CustomerService : CrudService<Customer, CustomerSaveDto, CustomerDetailDto, CustomerListing, CustomerListingFilter>, ICustomerService
    {
        private readonly ICustomerQuery _customerQuery;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository repository, ICustomerQuery query, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, query, unitOfWork)
        {
            _customerQuery = query;
            _mapper = mapper;
        }

        public Task<List<CustomerSynchronizationDto>> GetSynchronizationAsync(CustomerListingFilter filter, CancellationToken cancellationToken)
        {
            return _customerQuery.GetSynchronization(filter, cancellationToken);
        }

        protected override Customer CreateEntity(CustomerSaveDto dto)
        {
            return new Customer(dto.Name, dto.PersonType, dto.Document, dto.Email, dto.StateRegistration, _mapper.Map<IReadOnlyCollection<CustomerAddress>>(dto.Addresses));
        }

        protected override void UpdateEntity(CustomerSaveDto dto, Customer entity)
        {
            entity.Fill(dto.Name, dto.PersonType, dto.Document, dto.Email, dto.StateRegistration, _mapper.Map<IReadOnlyCollection<CustomerAddress>>(dto.Addresses));
        }

        protected override Task<bool> EntitiesCanBeDeleted(IEnumerable<long> ids)
        {
            return Task.FromResult(true);
        }
    }
}