using Application.Interfaces.Queries.Persons.Customers;
using Application.Interfaces.Services.Persons;
using Application.Persons.Customers.DataTransferObjects;
using Application.Persons.Customers.DataTransferObjects.Listings;
using Application.Shared.Services;
using AutoMapper;
using Domain.Persons.Customers.Entities;
using Domain.Persons.Customers.Repositories.Interfaces;
using Domain.Shared.Persistance.Interfaces;

namespace Application.Persons.Customers.Services
{
    public class CustomerService : CrudService<Customer, CustomerSaveDto, CustomerDetailDto, CustomerListing, CustomerListingFilter>, ICustomerService
    {
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository repository, ICustomerQuery query, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, query, unitOfWork)
        {
            _mapper = mapper;
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