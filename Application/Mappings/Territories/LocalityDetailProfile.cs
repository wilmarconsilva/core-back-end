using Application.DataTransferObjects.Entities;
using Application.DataTransferObjects.Territories;
using AutoMapper;
using Domain.Entities.Territories;

namespace Application.Mappings.Territories
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
