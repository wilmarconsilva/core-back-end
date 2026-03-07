namespace Application.DataTransferObjects.Commercial.Documents.Budgets.Validators
{
    public class BudgetSaveDtoValidator : DocumentSaveDtoValidator<BudgetProductSaveDto>
    {
        public BudgetSaveDtoValidator(BudgetProductSaveDtoValidator documentProductValidator) : base(documentProductValidator)
        {

        }
    }
}