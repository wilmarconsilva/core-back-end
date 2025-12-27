using Application.DataTransferObjects;
using Application.DataTransferObjects.Persons.Customers;
using Application.Interfaces.Services.Persons;
using AutoMapper;
using Domain.Entities.Persons.Customers;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;

namespace Application.Services.Persons
{
    public class CustomerAddressService : CrudService<CustomerAddress, CustomerAddressSaveDto>, ICustomerAddressService
    {
        public CustomerAddressService(ICustomerAddressRepository repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {

        }

        public async Task SaveByCustomersAsync(IEnumerable<EntityDto<Customer, CustomerSaveDto>> entitiesDtos, CancellationToken cancellationToken)
        {
            if (entitiesDtos == null || !entitiesDtos.Any())
                return;

            foreach (EntityDto<Customer, CustomerSaveDto> entityDto in entitiesDtos)
            {
                for (int i = 0; i < entityDto.Dto.Addresses.Count; i++)
                {
                    CustomerAddressSaveDto customerAddressSaveDto = entityDto.Dto.Addresses[i];

                    customerAddressSaveDto.CustomerId = entityDto.Entity.Id;
                }
            }

            await SaveAsync(entitiesDtos.SelectMany(ed => ed.Dto.Addresses), cancellationToken);
        } 
    }
}