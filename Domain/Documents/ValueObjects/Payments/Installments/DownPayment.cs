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
            ValueValidatorUtility.ValidatePositive(value, "O valor da entrada");
            ValueValidatorUtility.ValidatePositive(percentage, "A porcentagem da entrada");
            ValueValidatorUtility.ValidateEnum(method, "O método de pagamento da entrada");

            Value = Math.Round(value,2);
            Percentage = percentage;
            Method = method;
        }

        public bool ValidatePercentage(decimal valueTotal)
        {
            return Math.Round(valueTotal * (Percentage / 100), 2) == Value;
        }

        public bool ValidateValue(decimal valueTotal)
        {
            return Value < valueTotal;
        }
    }
}
