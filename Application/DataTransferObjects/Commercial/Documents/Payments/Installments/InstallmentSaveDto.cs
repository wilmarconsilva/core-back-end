namespace Application.DataTransferObjects.Commercial.Documents.Payments.Installments
{
    public class InstallmentSaveDto
    {
        public required int Quantity { get; set; }

        public DownPaymentSaveDto? DownPayment { get; set; }
    }
}
