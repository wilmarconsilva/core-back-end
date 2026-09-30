using Application.Persons.Customers.DataTransferObjects;
using Application.Shared.Mappings;
using Domain.Persons.Customers.Entities;

namespace Application.Persons.Customers.Mappings
{
    public class CustomerProfile : CrudProfile<CustomerSaveDto, Customer>
    {

    }
}