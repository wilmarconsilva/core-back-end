using Domain.Documents.ValueObjects.Payments.Enums;

namespace Application.Documents.DataTransferObjects.Reports
{
    public abstract class DocumentReport
    {
        public long Id { get; set; }

        public string CreationDateTime { get; set; } = null!;

        public string LastUpdateDateTime { get; set; } = null!;

        public decimal ValueSubtotal { get; set; }

        public decimal ValueTotal { get; set; }

        public PaymentMethod? PaymentMethod { get; set; }

        public string PaymentMethodDescription => PaymentMethod.GetDescription();

        public decimal PaymentDiscountValue { get; set; }

        public decimal? PaymentInstallmentQuantity { get; set; }

        public decimal? PaymentInstallmentValue { get; set; }

        public decimal? PaymentInstallmentDownPaymentValue { get; set; }

        public decimal? PaymentInstallmentDownPaymentPercentage { get; set; }

        public PaymentMethod? PaymentInstallmentDownPaymentMethod { get; set; }

        public string PaymentInstallmentDownPaymentMethodDescription => PaymentInstallmentDownPaymentMethod.GetDescription();

        public string? Observations { get; set; }

        public string SellerName { get; set; } = null!;

        public string CustomerName { get; set; } = null!;

        public string CustomerLocalityState { get; set; } = null!;
    }
}
