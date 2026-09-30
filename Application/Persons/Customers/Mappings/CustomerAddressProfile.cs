using Application.Persons.Customers.DataTransferObjects.CustomerAddresses;
using Application.Shared.Mappings;
using Domain.Persons.Customers.Entities;

namespace Application.Persons.Customers.Mappings
{
    public class CustomerAddressProfile : CrudProfile<CustomerAddressSaveDto, CustomerAddress>
    {

    }
}
