using Domain.Documents.ValueObjects.Payments.Enums;
using Domain.Shared.Utilities;

namespace Domain.Documents.ValueObjects.Payments.Installments
{
    public class DownPayment
    {
        public decimal Value { get; private init; }

        public decimal Percentage { get; private init; }

        public PaymentMethod Method { get; private init; }

        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        private DownPayment() { }

        public DownPayment(decimal value, decimal percentage, PaymentMethod method)
        {
            ValueValidatorUtility.ValidateValue(value, "O valor da entrada");
            ValueValidatorUtility.ValidateValue(percentage, "A porcentagem da entrada");
            ValueValidatorUtility.ValidateEnum(method, "O método de pagamento da entrada");

            Value = value;
            Percentage = percentage;
            Method = method;
        }

        public bool ValidatePercentage(decimal valueTotal)
        {
            return (valueTotal * (Percentage / 100)) == Value;
        }

        public bool ValidateValue(decimal valueTotal)
        {
            return Value < valueTotal;
        }
    }
}
