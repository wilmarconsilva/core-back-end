using Application.DataTransferObjects.Persons.Customers;
using AutoMapper;
using Domain.Entities.Persons.Customers;

namespace Application.Mappings.Persons.Customers.Details
{
    public class CustomerDetailProfile : Profile
    {
        public CustomerDetailProfile()
        {
            CreateMap<Customer, CustomerDetailDto>();
        }
    }
}
