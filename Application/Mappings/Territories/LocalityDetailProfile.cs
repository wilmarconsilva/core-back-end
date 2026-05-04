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
                    s.State != null
                        ? new StateReferenceDto
                        {
                            Id = s.State.Id,
                            Acronym = s.State.Acronym,
                            Description = s.State.Description,
                        }
                        : null));
        }
    }
}
