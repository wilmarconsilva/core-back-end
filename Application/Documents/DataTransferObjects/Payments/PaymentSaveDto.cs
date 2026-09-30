using Application.Documents.DataTransferObjects.Payments.Installments;

namespace Application.Documents.DataTransferObjects.Payments
{
    public abstract class PaymentSaveDto
    {
        public decimal DiscountValue { get; set; }

        public InstallmentSaveDto? Installment { get; set; }
    }
}
