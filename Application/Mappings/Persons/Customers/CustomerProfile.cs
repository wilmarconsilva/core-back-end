using Application.Persons.Customers.DataTransferObjects;
using Domain.Persons.Customers.Entities;

namespace Application.Mappings.Persons.Customers
{
    public class CustomerProfile : CrudProfile<CustomerSaveDto, Customer>
    {

    }
}