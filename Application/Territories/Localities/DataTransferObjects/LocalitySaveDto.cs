using Application.Shared.DataTransferObjects.Entities;

namespace Application.Territories.Localities.DataTransferObjects
{
    public class LocalitySaveDto : EntitySaveDto
    {
        public required string Description { get; set; }

        public long? StateId { get; set; }
    }
}
