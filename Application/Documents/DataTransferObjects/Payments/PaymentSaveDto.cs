using Application.Documents.DataTransferObjects.Payments.Installments;
using Domain.Documents.ValueObjects.Payments.Enums;

namespace Application.Documents.DataTransferObjects.Payments
{
    public class PaymentSaveDto
    {
        public PaymentMethod? Method { get; set; }

        public decimal DiscountValue { get; set; }

        public InstallmentSaveDto? Installment { get; set; }
    }
}
