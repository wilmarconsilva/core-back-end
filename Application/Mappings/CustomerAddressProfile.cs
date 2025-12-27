using Application.DataTransferObjects.Persons.Customers;
using Domain.Entities.Persons.Customers;
using Domain.Entities.Territories;

namespace Application.Mappings
{
    public class CustomerAddressProfile : CrudProfile<CustomerAddressSaveDto, CustomerAddress>
    {
        public CustomerAddressProfile()
        {
            CreateMap<CustomerAddressSaveDto, CustomerAddress>()
                .ForMember(dest => dest.Locality,
                    opt => opt.MapFrom(src =>
                    src.LocalityId > 0
                        ? new Locality(src.LocalityId, string.Empty)
                        : null
                    )
                );

            CreateMap<CustomerAddress, CustomerAddressSaveDto>()
                .ForMember(
                    dest => dest.LocalityId,
                    opt => opt.MapFrom(src => src.Locality.Id)
                );
        }
    }
}