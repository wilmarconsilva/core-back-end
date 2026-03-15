using Application.DataTransferObjects.Commercial.Documents.Payments;

namespace Application.DataTransferObjects.Commercial.Documents
{
    public abstract class DocumentSaveDto<TDocumentProductSaveDto> : EntitySaveDto where TDocumentProductSaveDto : DocumentProductSaveDto
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public override long Id { get; set; }

        public required long UserId { get; set; }

        public required long CustomerId { get; set; }

        public long? BudgetId { get; set; }

        public required PaymentSaveDto Payment { get; set; }

        public string? Observations { get; set; }

        public byte[]? CustomerSignatureImage { get; set; }

        public required List<TDocumentProductSaveDto> Products { get; set; }
    }
}