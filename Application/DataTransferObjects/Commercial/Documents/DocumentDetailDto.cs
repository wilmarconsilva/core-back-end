using Application.DataTransferObjects.Commercial.Documents.DocumentsProducts;
using Application.DataTransferObjects.Commercial.Documents.Payments;
using Application.DataTransferObjects.Entities;

namespace Application.DataTransferObjects.Commercial.Documents
{
    public class DocumentDetailDto : EntitySaveDto
    {
        public override long Id { get; set; }

        public required long UserId { get; set; }

        public required EntityReferenceDto Customer { get; set; }

        public required PaymentSaveDto Payment { get; set; }

        public string? Observations { get; set; }

        public byte[]? CustomerSignatureImage { get; set; }

        public required List<DocumentProductDetailDto> Products { get; set; }
    }
}
