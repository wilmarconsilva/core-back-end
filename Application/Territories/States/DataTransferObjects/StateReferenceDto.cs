using Application.Shared.DataTransferObjects.Entities;

namespace Application.Territories.States.DataTransferObjects
{
    public class StateReferenceDto : EntityReferenceDto
    {
        public required string Acronym { get; init; }
    }
}
