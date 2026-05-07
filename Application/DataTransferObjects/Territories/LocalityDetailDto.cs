using Application.DataTransferObjects.Entities;

namespace Application.DataTransferObjects.Territories
{
    public class LocalityDetailDto : EntitySaveDto
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public override long Id { get; set; }

        public required string Description { get; set; }

        public EntityReferenceDto? State { get; set; }
    }
}
