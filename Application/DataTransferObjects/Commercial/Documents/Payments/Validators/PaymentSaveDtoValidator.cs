using Application.DataTransferObjects.Commercial.Documents.Payments;
using Application.DataTransferObjects.Commercial.Documents.Payments.Installments;
using FluentValidation;

namespace Application.DataTransferObjects.Commercial.Documents.Budgets.Validators
{
    public class PaymentSaveDtoValidator : Validator<PaymentSaveDto>
    {
        public PaymentSaveDtoValidator(IValidator<InstallmentSaveDto> installmentSaveDtoValidator)
        {
            RequiredEnum(p => p.Method);

            RequiredNumber(p => p.DiscountValue);

            When(p => p.Installment is not null, () =>
            {
                RuleFor(p => p.Installment).SetValidator(installmentSaveDtoValidator!);
            });
        }
    }
}