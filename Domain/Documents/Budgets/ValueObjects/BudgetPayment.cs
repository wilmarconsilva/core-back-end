using Domain.Documents.ValueObjects.Payments;
using Domain.Documents.ValueObjects.Payments.Enums;
using Domain.Documents.ValueObjects.Payments.Installments;
using Domain.Shared.Utilities;

namespace Domain.Documents.Budgets.ValueObjects
{
    public class BudgetPayment : Payment
    {
        public PaymentMethod? Method { get; private init; }

        public BudgetPayment(PaymentMethod? method, decimal discountValue, Installment? installment)
            : base(discountValue, installment)
        {
            if (method.HasValue)
            {
                ValueValidatorUtility.ValidateEnum(method.Value, $"O método de pagamento");
            }

            Method = method;
        }

        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        public BudgetPayment() { }
    }
}
