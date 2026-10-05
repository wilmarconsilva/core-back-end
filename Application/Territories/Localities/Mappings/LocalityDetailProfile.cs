using Application.Territories.Localities.DataTransferObjects;
using Application.Territories.States.DataTransferObjects;
using AutoMapper;
using Domain.Territories.Localities.Entities;

namespace Application.Territories.Localities.Mappings
{
    public class LocalityDetailProfile : Profile
    {
        public LocalityDetailProfile()
        {
            // Every locality has a state (state_id is NOT NULL in the database).
            CreateMap<Locality, LocalityDetailDto>()
                .ForMember(d => d.State, opt => opt.MapFrom(s =>
                    new StateReferenceDto
                    {
                        Id = s.StateId!.Value,
                        Description = s.State!.Description,
                        Acronym = s.State.Acronym,
                    }));
        }
    }
}
