using Application.DataTransferObjects.Entities;
using Application.Persons.Customers.DataTransferObjects.CustomerAddresses;
using AutoMapper;
using Domain.Persons.Customers.Entities;

namespace Application.Mappings.Persons.Customers.Details
{
    public class CustomerAddressDetailProfile : Profile
    {
        public CustomerAddressDetailProfile()
        {
            CreateMap<CustomerAddress, CustomerAddressDetailDto>()
                .ForMember(d => d.Locality, opt => opt.MapFrom(s =>
                    new EntityReferenceDto
                    {
                        Id = s.LocalityId,
                        Description = s.Locality!.Description,
                    }));
        }
    }
}
