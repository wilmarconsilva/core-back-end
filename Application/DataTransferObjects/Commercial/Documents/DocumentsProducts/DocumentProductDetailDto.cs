using Application.DataTransferObjects.Entities;

namespace Application.DataTransferObjects.Commercial.Documents.DocumentsProducts
{
    public class DocumentProductDetailDto : EntitySaveDto
    {
        public required EntityReferenceDto Product { get; set; }

        public required decimal Quantity { get; set; }

        public required decimal ValueUnit { get; set; }

        public required decimal ValueTotal { get; set; }
    }
}
