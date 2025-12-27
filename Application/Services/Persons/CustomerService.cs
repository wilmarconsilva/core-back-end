using Application.DataTransferObjects;
using Application.DataTransferObjects.Persons.Customers;
using Application.Interfaces.Services.Persons;
using AutoMapper;
using Domain.Entities.Persons.Customers;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;

namespace Application.Services.Persons
{
    public class CustomerService : CrudService<Customer, CustomerSaveDto>, ICustomerService
    {
        private readonly ICustomerAddressService _customerAddressService;

        public CustomerService(ICustomerRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ICustomerAddressService customerAddressService) : base(repository, unitOfWork, mapper)
        {
            _customerAddressService = customerAddressService;
        }

        protected override Task<string?> ValidateBusinessRulesForSaveAsync(EntityDto<Customer, CustomerSaveDto> entityDto, CancellationToken cancellationToken)
        {
            string? message = null;

            if (entityDto.Dto.Addresses.Count == 0)
                message = $"É obrigatório informar ao menos um endereço para o Cliente: {entityDto.Entity.Description}.";

            return Task.FromResult(message);
        }

        protected override async Task SaveDependentEntitiesAsync(IEnumerable<EntityDto<Customer, CustomerSaveDto>> entitiesDtos, CancellationToken cancellationToken)
        {
            await _customerAddressService.SaveByCustomersAsync(entitiesDtos, cancellationToken);
        }
    }
}