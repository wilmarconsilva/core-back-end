using Application.Documents.DataTransferObjects.Payments;
using Domain.Documents.ValueObjects.Payments.Enums;

namespace Application.Documents.Budgets.DataTransferObjects
{
    public class BudgetPaymentSaveDto : PaymentSaveDto
    {
        public PaymentMethod? Method { get; set; }
    }
}
