using Application.DataTransferObjects.Commercial.Documents.Payments.Installments;
using Domain.ValueObjects.Enums;

namespace Application.DataTransferObjects.Commercial.Documents.Payments
{
    public class PaymentSaveDto
    {
        public required PaymentMethod Method { get; set; }

        public required decimal DiscountValue { get; set; }

        public InstallmentSaveDto? Installment { get; set; }
    }
}
