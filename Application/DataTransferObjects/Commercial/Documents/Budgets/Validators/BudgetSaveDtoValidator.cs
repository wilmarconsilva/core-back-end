using FluentValidation;

namespace Application.DataTransferObjects.Commercial.Documents.Budgets.Validators
{
    public class BudgetSaveDtoValidator : Validator<BudgetSaveDto>
    {
        public BudgetSaveDtoValidator(IValidator<BudgetProductSaveDto> budgetProductValidator)
        {
            RequiredNumber(b => b.UserId);
            RequiredNumber(b => b.CustomerId);
            RequiredObject(b => b.Payment);

            RuleForEach(b => b.Products).SetValidator(budgetProductValidator);
        }
    }
}