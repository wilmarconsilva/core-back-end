using Application.Shared.DataTransferObjects.Entities;

namespace Application.Documents.DataTransferObjects.DocumentsProducts
{
    public class DocumentProductDetailDto : EntitySaveDto
    {
        public required EntityReferenceDto Product { get; set; }

        public required decimal Quantity { get; set; }

        public required decimal ValueUnit { get; set; }
    }
}
