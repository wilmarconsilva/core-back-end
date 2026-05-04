using Application.DataTransferObjects.Persons.Customers;
using AutoMapper;
using Domain.Entities.Persons.Customers;

namespace Application.Mappings.Persons.Customers
{
    public class CustomerAddressProfile : Profile
    {
        public CustomerAddressProfile()
        {
            CreateMap<CustomerAddressSaveDto, CustomerAddress>()
                .ForMember(d => d.Locality, opt => opt.Ignore());

            CreateMap<CustomerAddress, CustomerAddressSaveDto>();
        }
    }
}
