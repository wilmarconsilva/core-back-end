using Application.DataTransferObjects;
using Application.DataTransferObjects.Persons.Customers;
using Application.Interfaces.Services.Persons;
using Domain.Entities.Persons.Customers;

namespace Application.Services.Persons.Customers
{
    public class CustomerAddressByCustomerService : CrudByRelatedEntityService<CustomerAddressSaveDto, Customer, CustomerSaveDto>, ICustomerAddressByCustomerService
    {
        public CustomerAddressByCustomerService(ICustomerAddressService service) : base(service)
        {

        }

        protected override List<CustomerAddressSaveDto> GetEntitiesSaveDtosFromRelatedEntity(EntityDto<Customer, CustomerSaveDto> relatedEntityDto)
        {
            List<CustomerAddressSaveDto> dtos = new List<CustomerAddressSaveDto>();

            CustomerSaveDto customerDto = relatedEntityDto.Dto;

            for (int i = 0; i < customerDto.Addresses.Count; i++)
            {
                CustomerAddressSaveDto dto = customerDto.Addresses[i];

                dto.CustomerId = relatedEntityDto.Entity.Id;

                dtos.Add(dto);
            }

            return dtos;
        }
    }
}