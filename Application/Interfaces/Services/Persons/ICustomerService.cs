using Application.DataTransferObjects.Persons.Customers;
using Domain.Entities.Persons.Customers;

namespace Application.Interfaces.Services.Persons
{
    public interface ICustomerService : ICrudService<Customer, CustomerSaveDto>
    {
        
    }
}