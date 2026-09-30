using Application.Documents.DataTransferObjects;

namespace Application.Documents.Budgets.DataTransferObjects
{
    public class BudgetDetailDto : DocumentDetailDto
    {
        public required BudgetPaymentSaveDto Payment { get; set; }
    }
}
