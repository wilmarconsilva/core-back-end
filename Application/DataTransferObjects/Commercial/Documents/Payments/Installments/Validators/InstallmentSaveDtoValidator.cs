using FluentValidation;

namespace Application.DataTransferObjects.Commercial.Documents.Payments.Installments
{
    public class InstallmentSaveDtoValidator : Validator<InstallmentSaveDto>
    {
        public InstallmentSaveDtoValidator(IValidator<DownPaymentSaveDto> downPaymentSaveDtoValidator)
        {
            RequiredNumber(i => i.Value);
            RequiredNumber(i => i.Quantity);
            When(i => i.DownPayment is not null, () =>
            {
                RuleFor(i => i.DownPayment).SetValidator(downPaymentSaveDtoValidator!);
            });
        }
    }
}