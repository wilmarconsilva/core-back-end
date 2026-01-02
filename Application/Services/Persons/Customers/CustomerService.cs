using Application.DataTransferObjects;
using Application.DataTransferObjects.Persons.Customers;
using Application.Interfaces.Services.Persons;
using AutoMapper;
using Domain.Entities.Persons.Customers;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;

namespace Application.Services.Persons.Customers
{
    public class CustomerService : CrudService<Customer, CustomerSaveDto>, ICustomerService
    {
        private readonly ICustomerAddressByCustomerService _customerAddressService;

        public CustomerService(ICustomerRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ICustomerAddressByCustomerService customerAddressService) : base(repository, unitOfWork, mapper)
        {
            _customerAddressService = customerAddressService;
        }

        protected override async Task SaveRelatedEntitiesAsync(EntityDto<Customer, CustomerSaveDto>[] entitiesDtos, CancellationToken cancellationToken)
        {
            await _customerAddressService.SaveByRelatedEntitiesAsync(entitiesDtos, cancellationToken);
        }
    }
}