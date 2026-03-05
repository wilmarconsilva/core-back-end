using Application.DataTransferObjects;

namespace Application.DataTransferObjects.Commercial.Documents.Budgets.Validators
{
    public class BudgetProductSaveDtoValidator : Validator<BudgetProductSaveDto>
    {
        public BudgetProductSaveDtoValidator()
        {
            RequiredNumber(bp => bp.ProductId);
            RequiredNumber(bp => bp.Quantity);
            RequiredNumber(bp => bp.ValueUnit);
            RequiredNumber(bp => bp.ValueTotal);
        }
    }
}