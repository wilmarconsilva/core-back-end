namespace Application.DataTransferObjects.Commercial.Documents
{
    public abstract class DocumentReport<TDocumentProductReport> where TDocumentProductReport : DocumentProductReport
    {
        public string CreationDateTimeDescription { get; set; } = null!;

        public string LastUpdateDateTimeDescription { get; set; } = null!;

        public decimal ValueSubtotal { get; set; }

        public decimal ValueTotal { get; set; }

        public string PaymentMethodDescription { get; set; } = null!;

        public decimal PaymentDiscountValue { get; set; }

        public decimal? PaymentInstallmentQuantity { get; set; }

        public decimal? PaymentInstallmentValue { get; set; }

        public decimal? PaymentInstallmentDownPaymentValue { get; set; }

        public decimal? PaymentInstallmentDownPaymentPercentage { get; set; }

        public string? PaymentInstallmentDownPaymentMethodDescription { get; set; }

        public string? Observations { get; set; }

        public string UserName { get; set; } = null!;

        public string CustomerName { get; set; } = null!;

        public string CustomerAddressCityState { get; set; } = null!;
    }
}
