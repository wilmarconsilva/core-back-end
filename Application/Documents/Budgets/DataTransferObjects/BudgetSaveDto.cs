using Application.Documents.DataTransferObjects;

namespace Application.Documents.Budgets.DataTransferObjects
{
    public class BudgetSaveDto : DocumentSaveDto<BudgetProductSaveDto>
    {
        public required BudgetPaymentSaveDto Payment { get; set; }
    }
}