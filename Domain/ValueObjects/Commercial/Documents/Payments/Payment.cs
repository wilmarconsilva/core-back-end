using Domain.Utilities.Entities;
using Domain.ValueObjects.Commercial.Documents.Payments.Installments;
using Domain.ValueObjects.Enums;

namespace Domain.ValueObjects.Commercial.Documents.Payments
{
    public class Payment
    {
        public PaymentMethod Method { get; private init; }

        public decimal DiscountValue { get; private init; }

        public bool HasInstallment { get; private init; }

        public Installment? Installment { get; private init; }

        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        private Payment() { }

        public Payment(PaymentMethod method, decimal discountValue, Installment? installment)
        {
            Validator.ValidateEnum(method, nameof(Method));
            Validator.ValidateValue(discountValue, nameof(DiscountValue));

            Method = method;
            DiscountValue = discountValue;
            Installment = installment;

            if (installment != null)
                HasInstallment = true;
        }
    }
}
