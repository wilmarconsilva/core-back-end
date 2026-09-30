namespace Application.Documents.DataTransferObjects.Payments.Installments
{
    public class InstallmentSaveDto
    {
        public required int Quantity { get; set; }

        public DownPaymentSaveDto? DownPayment { get; set; }
    }
}
