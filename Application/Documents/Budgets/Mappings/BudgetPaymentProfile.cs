using Application.Documents.Budgets.DataTransferObjects;
using Application.Shared.Mappings;
using Domain.Documents.Budgets.ValueObjects;

namespace Application.Documents.Budgets.Mappings
{
    public class BudgetPaymentProfile : CrudProfile<BudgetPaymentSaveDto, BudgetPayment>
    {
    }
}
