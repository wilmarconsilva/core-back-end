using Domain.Documents.ValueObjects.Payments.Enums;
using Domain.Documents.ValueObjects.Payments.Installments;
using Domain.Shared.Utilities;

namespace Domain.Documents.ValueObjects.Payments
{
    public class Payment
    {
        public PaymentMethod? Method { get; private init; }

        public decimal DiscountValue { get; private init; }

        public bool HasInstallment { get; private init; }

        public Installment? Installment { get; private init; }

        public Payment(PaymentMethod? method, decimal discountValue, Installment? installment)
        {
            if (method.HasValue)
            {
                ValueValidatorUtility.ValidateEnum(method.Value, $"O método de pagamento");
            }

            ValueValidatorUtility.ValidateNotNegative(discountValue, "O valor do desconto");

            Method = method;
            DiscountValue = discountValue;
            Installment = installment;

            if (installment != null)
                HasInstallment = true;
        }

        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        private Payment() { }
    }
}
