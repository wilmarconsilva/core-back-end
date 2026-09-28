using Application.Persons.Customers.DataTransferObjects;
using AutoMapper;
using Domain.Persons.Customers.Entities;

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
