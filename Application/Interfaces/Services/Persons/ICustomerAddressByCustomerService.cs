using Application.DataTransferObjects.Persons.Customers;
using Domain.Entities.Persons.Customers;

namespace Application.Interfaces.Services.Persons
{
    public interface ICustomerAddressByCustomerService : IRelatedEntityCrudService<Customer, CustomerSaveDto>
    {

    }
}