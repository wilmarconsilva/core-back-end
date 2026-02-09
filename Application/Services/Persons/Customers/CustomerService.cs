using Application.DataTransferObjects;
using Application.DataTransferObjects.Persons.Customers;
using Application.Interfaces.Queries.Persons.Customers;
using Application.Interfaces.Services.Persons;
using AutoMapper;
using Domain.Entities.Persons.Customers;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;

namespace Application.Services.Persons.Customers
{
    public class CustomerService : CrudService<Customer, CustomerSaveDto>, ICustomerService
    {
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository repository, ICustomerQuery query, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, query, unitOfWork, mapper)
        {
            _mapper = mapper;
        }

        protected override EntityDto<Customer, CustomerSaveDto> TransformToEntity(CustomerSaveDto dto)
        {
            for (int i = 0; i < dto.Addresses.Count; i++)
            {
                CustomerAddressSaveDto dtoAddress = dto.Addresses[i];

                dtoAddress.CustomerId = dto.Id;
            }

            return new EntityDto<Customer, CustomerSaveDto>
            {
                Entity = new Customer(dto.Name, dto.PersonType, dto.Document, dto.Email, dto.StateRegistration, _mapper.Map<IEnumerable<CustomerAddress>>(dto.Addresses)),
                Dto = dto
            };
        }

        protected virtual async Task<IEnumerable<Customer>> GetEntitiesToSave(EntityDto<Customer, CustomerSaveDto>[] entitiesDtos)
        {

        }
    }
}