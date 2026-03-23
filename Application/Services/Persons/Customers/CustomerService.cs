using Application.DataTransferObjects.Persons.Customers;
using Application.DataTransferObjects.Persons.Customers.Listings;
using Application.Interfaces.Queries.Persons.Customers;
using Application.Interfaces.Services.Persons;
using AutoMapper;
using Domain.Entities.Persons.Customers;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;

namespace Application.Services.Persons.Customers
{
    public class CustomerService : CrudService<Customer, CustomerSaveDto, CustomerListing, CustomerListingFilter>, ICustomerService
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