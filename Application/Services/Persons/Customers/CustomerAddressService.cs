using Application.DataTransferObjects;
using Application.DataTransferObjects.Persons.Customers;
using Application.Interfaces.Services.Persons;
using AutoMapper;
using Domain.Entities.Persons.Customers;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;

namespace Application.Services.Persons.Customers
{
    public class CustomerAddressService : CrudService<CustomerAddress, CustomerAddressSaveDto>, ICustomerAddressService
    {
        public CustomerAddressService(ICustomerAddressRepository repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {

        }

        protected override Task<string?> ValidateBusinessRulesForSaveAsync(EntityDto<CustomerAddress, CustomerAddressSaveDto> entityDto, CancellationToken cancellationToken)
        {
            if (entityDto.Entity.LocalityId <= 0)
                return Task.FromResult<string?>($"É obrigatório informar uma Localidade para o Endereço: {entityDto.Entity.Description}.");

            if (entityDto.Entity.CustomerId <= 0)
                return Task.FromResult<string?>($"É obrigatório informar um Cliente para o Endereço: {entityDto.Entity.Description}.");

            return Task.FromResult<string?>(null);
        }
    }
}