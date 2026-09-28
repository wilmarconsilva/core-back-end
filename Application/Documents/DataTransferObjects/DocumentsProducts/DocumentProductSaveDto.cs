using Application.Shared.DataTransferObjects.Entities;

namespace Application.Documents.DataTransferObjects.DocumentsProducts
{
    public abstract class DocumentProductSaveDto : EntitySaveDto
    {
        public required long ProductId { get; set; }

        public required decimal Quantity { get; set; }

        public required decimal ValueUnit { get; set; }

        public required decimal ValueTotal { get; set; }
    }
}
