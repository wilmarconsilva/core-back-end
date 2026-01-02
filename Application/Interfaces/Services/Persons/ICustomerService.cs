using Application.DataTransferObjects.Persons.Customers;

namespace Application.Interfaces.Services.Persons
{
    public interface ICustomerService : ICrudService<CustomerSaveDto>
    {
        
    }
}