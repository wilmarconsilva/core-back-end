using Domain.Documents.ValueObjects.Payments.Enums;

namespace Application.Documents.DataTransferObjects.Payments.Installments
{
    public class DownPaymentSaveDto
    {
        public required decimal Value { get; set; }

        public required decimal Percentage { get; set; }

        public required PaymentMethod Method { get; set; }
    }
}
