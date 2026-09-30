using Application.Documents.DataTransferObjects.DocumentsProducts;
using Application.Shared.DataTransferObjects.Entities;

namespace Application.Documents.DataTransferObjects
{
    public abstract class DocumentSaveDto<TDocumentProductSaveDto> : EntitySaveDto where TDocumentProductSaveDto : DocumentProductSaveDto
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public override long Id { get; set; }

        public required DateTime CreationDateTime { get; set; }

        public required long CustomerId { get; set; }

        public long? BudgetId { get; set; }

        public string? Observations { get; set; }

        public byte[]? CustomerSignatureImage { get; set; }

        public byte[]? PDF { get; set; }

        public required List<TDocumentProductSaveDto> Products { get; set; }
    }
}