using Domain.ValueObjects.Commercial.Documents.Payments;

namespace Application.DataTransferObjects.Commercial.Documents.Budgets
{
    public class BudgetSaveDto : EntitySaveDto
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public override long Id { get; set; }

        public required long UserId { get; set; }

        public required long CustomerId { get; set; }

        public required Payment Payment { get; set; }

        public string? Observations { get; set; }

        public byte[]? CustomerSignatureImage { get; set; }

        public required List<BudgetProductSaveDto> Products { get; set; }
    }
}
