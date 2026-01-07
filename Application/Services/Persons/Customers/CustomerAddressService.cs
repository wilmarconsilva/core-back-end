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
        private readonly ICustomerAddressRepository _repository;

        public CustomerAddressService(ICustomerAddressRepository repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
            _repository = repository;
        }

        public async Task SaveByCustomersAsync(EntityDto<Customer, CustomerSaveDto>[] relatedEntityDtos, CancellationToken cancellationToken)
        {
            if (relatedEntityDtos == null || relatedEntityDtos.Length == 0)
                return;

            List<CustomerAddressSaveDto> dtos = new();
            List<long> customerIds = new();

            for (int i = 0; i < relatedEntityDtos.Length; i++)
            {
                EntityDto<Customer, CustomerSaveDto> relatedEntityDto = relatedEntityDtos[i];

                CustomerSaveDto customerDto = relatedEntityDto.Dto;
                Customer customer = relatedEntityDto.Entity;

                for (int k = 0; k < customerDto.Addresses.Count; k++)
                {
                    CustomerAddressSaveDto dto = customerDto.Addresses[k];

                    dto.CustomerId = customer.Id;

                    dtos.Add(dto);
                }

                customerIds.Add(customer.Id);
            }

            if (dtos.Count == 0)
                return;

            IReadOnlyList<long> existingIds = await _repository.GetIdsByCustomerAsync(customerIds, cancellationToken);

            IEnumerable<long> idsDelete = existingIds.Except(dtos.Where(dto => dto.Id > 0).Select(dto => dto.Id));
            if (idsDelete.Any())
                await _repository.DeleteAsync(idsDelete, cancellationToken);

            await SaveAsync(dtos, cancellationToken);
        }
    }
}