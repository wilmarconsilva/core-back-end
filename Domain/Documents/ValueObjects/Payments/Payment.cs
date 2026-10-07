using Domain.Documents.ValueObjects.Payments.Installments;
using Domain.Shared.Utilities;

namespace Domain.Documents.ValueObjects.Payments
{
    public abstract class Payment
    {
        public decimal DiscountValue { get; private init; }

        public bool HasInstallment { get; private init; }

        public Installment? Installment { get; private init; }

        protected Payment(decimal discountValue, Installment? installment)
        {
            ValueValidatorUtility.ValidateNotNegative(discountValue, "O valor do desconto");

            DiscountValue = discountValue;
            Installment = installment;

            if (installment != null)
                HasInstallment = true;
        }

        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        protected Payment() { }
    }
}
