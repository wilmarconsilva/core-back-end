using Domain.ValueObjects.Commercial.Documents.Payments.Installments;

namespace Application.DataTransferObjects.Commercial.Documents.Payments.Installments
{
    public class InstallmentSaveDto
    {
        public required decimal Value { get; set; }

        public required int Quantity { get; set; }

        public DownPaymentSaveDto? DownPayment { get; set; }
    }
}
