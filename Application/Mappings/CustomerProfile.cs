using Application.DataTransferObjects.Persons.Customers;
using Domain.Entities.Persons.Customers;

namespace Application.Mappings
{
    public class CustomerProfile : CrudProfile<CustomerSaveDto, Customer>
    {
        
    }
}