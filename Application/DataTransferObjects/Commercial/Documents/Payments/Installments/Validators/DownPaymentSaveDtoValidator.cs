namespace Application.DataTransferObjects.Commercial.Documents.Payments
{
    public class DownPaymentSaveDtoValidator : Validator<DownPaymentSaveDto>
    {
        public DownPaymentSaveDtoValidator()
        {
            RequiredNumber(c => c.Value);
            RequiredNumber(c => c.Percentage);

            RequiredEnum(c => c.Method);
        }
    }
}
