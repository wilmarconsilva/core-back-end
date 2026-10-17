using Application.Documents.DataTransferObjects.DocumentsProducts;
using Application.Documents.DataTransferObjects.Payments;
using Application.Shared.DataTransferObjects.Entities;

namespace Application.Documents.DataTransferObjects
{
    public abstract class DocumentDetailDto : EntitySaveDto
    {
        public override long Id { get; set; }

        public required long SellerId { get; set; }

        public required DateTime CreationDateTime { get; set; }

        public required EntityReferenceDto Customer { get; set; }

        public required PaymentSaveDto Payment { get; set; }

        public string? Observations { get; set; }

        public byte[]? CustomerSignatureImage { get; set; }

        public required List<DocumentProductDetailDto> Products { get; set; }
    }
}
