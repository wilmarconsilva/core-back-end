using Application.DataTransferObjects.Commercial.Documents.Payments;
using Application.DataTransferObjects.Entities;
using Application.DataTransferObjects.Persons.Customers;

namespace Application.DataTransferObjects.Commercial.Documents.Budgets
{
    public class BudgetDetailDto : EntitySaveDto
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public override long Id { get; set; }

        public required long UserId { get; set; }

        public required CustomerReferenceDto Customer { get; set; }

        public required PaymentSaveDto Payment { get; set; }

        public string? Observations { get; set; }

        public byte[]? CustomerSignatureImage { get; set; }

        public required List<BudgetProductDetailDto> Products { get; set; }
    }
}
