using Domain.ValueObjects.Enums;

namespace Application.DataTransferObjects.Commercial.Documents.Payments
{
    public class DownPaymentSaveDto
    {
        public required decimal Value { get; set; }

        public required decimal Percentage { get; set; }

        public required PaymentMethod Method { get; set; }
    }
}
