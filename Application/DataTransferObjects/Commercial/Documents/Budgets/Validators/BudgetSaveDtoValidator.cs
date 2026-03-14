using Application.DataTransferObjects.Commercial.Documents.Payments;
using FluentValidation;

namespace Application.DataTransferObjects.Commercial.Documents.Budgets.Validators
{
    public class BudgetSaveDtoValidator : DocumentSaveDtoValidator<BudgetSaveDto, BudgetProductSaveDto>
    {
        public BudgetSaveDtoValidator(BudgetProductSaveDtoValidator documentProductValidator, IValidator<PaymentSaveDto> paymentSaveDtoValidator) : base(documentProductValidator, paymentSaveDtoValidator)
        {

        }
    }
}