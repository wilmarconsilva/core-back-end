using Application.Shared.DataTransferObjects.Entities;
using Application.Territories.States.DataTransferObjects;

namespace Application.Territories.Localities.DataTransferObjects
{
    public class LocalityDetailDto : EntitySaveDto
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public override long Id { get; set; }

        public required string Description { get; set; }

        public required StateReferenceDto State { get; set; }
    }
}
