using Application.DataTransferObjects;
using Application.DataTransferObjects.Persons.Customers;
using Domain.Entities.Persons.Customers;

namespace Application.Interfaces.Services.Persons
{
    public interface ICustomerAddressService : ICrudService<CustomerAddress,CustomerAddressSaveDto>
    {

    }
}