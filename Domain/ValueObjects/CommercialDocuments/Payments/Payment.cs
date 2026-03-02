using Domain.Utilities.Entities;
using Domain.ValueObjects.CommercialDocuments.Payments.Installments;
using Domain.ValueObjects.Enums;

namespace Domain.ValueObjects.CommercialDocuments.Payments
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

        public Payment(PaymentMethod method, decimal discountValue, Installment? installments)
        {
            Validator.ValidateEnum(method, nameof(Method));
            Validator.ValidateValue(discountValue, nameof(DiscountValue));

            Method = method;
            DiscountValue = discountValue;
            Installment = installments;

            if (installments != null)
                HasInstallment = true;
        }
    }
}
