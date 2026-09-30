using Application.Shared.DataTransferObjects.Entities;
using Application.Territories.Localities.DataTransferObjects;
using AutoMapper;
using Domain.Territories.Localities.Entities;

namespace Application.Territories.Localities.Mappings
{
    public class LocalityDetailProfile : Profile
    {
        public LocalityDetailProfile()
        {
            CreateMap<Locality, LocalityDetailDto>()
                .ForMember(d => d.State, opt => opt.MapFrom(s =>
                    s.StateId != null ? new EntityReferenceDto
                    {
                        Id = s.StateId.Value,
                        Description = s.State!.Description,
                    } : null));
        }
    }
}
