using Application.DataTransferObjects.Localities;
using Application.DataTransferObjects.Persons.Customers;
using AutoMapper;
using Domain.Entities.Persons.Customers;

namespace Application.Mappings.Persons.Customers
{
    public class CustomerDetailProfile : Profile
    {
        public CustomerDetailProfile()
        {
            CreateMap<Customer, CustomerDetailDto>();

            CreateMap<CustomerAddress, CustomerAddressDetailDto>()
                .ForMember(d => d.Locality, opt => opt.MapFrom(s =>
                    new LocalityReferenceDto
                    {
                        Id = s.Locality != null ? s.Locality.Id : s.LocalityId,
                        Description = s.Locality != null ? s.Locality.Description : null,
                    }));
        }
    }
}
