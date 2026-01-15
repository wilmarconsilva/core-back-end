using Application.DataTransferObjects;
using Application.DataTransferObjects.Persons.Customers;
using Application.Interfaces.Queries.Persons.Customers;
using Application.Interfaces.Services.Persons;
using AutoMapper;
using Domain.Entities.Persons.Customers;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;

namespace Application.Services.Persons.Customers
{
    public class CustomerService : CrudService<Customer, CustomerSaveDto>, ICustomerService
    {

        public CustomerService(ICustomerRepository repository, ICustomerQuery query, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, query, unitOfWork, mapper)
        {

        }
    }
}