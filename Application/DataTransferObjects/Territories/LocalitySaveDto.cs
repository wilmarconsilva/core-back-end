using Application.DataTransferObjects.Entities;

namespace Application.DataTransferObjects.Territories
{
    public class LocalitySaveDto : EntitySaveDto
    {
        public required string Description { get; set; }

        public long? StateId { get; set; }
    }
}
