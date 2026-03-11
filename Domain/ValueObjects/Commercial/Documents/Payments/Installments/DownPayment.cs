using Domain.Utilities.Entities;
using Domain.ValueObjects.Enums;

namespace Domain.ValueObjects.Commercial.Documents.Payments.Installments
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
            string downPaymentFullName = $"{typeof(DownPayment).FullName}";

            Validator.ValidateValue(value, $"{downPaymentFullName}.{nameof(Value)}");
            Validator.ValidateValue(percentage, $"{downPaymentFullName}.{nameof(Value)}");
            Validator.ValidateEnum(method, $"{downPaymentFullName}.{nameof(Value)}");

            Value = value;
            Percentage = percentage;
            Method = method;
        }

        public bool ValidatePercentageCorrespondsToValue(decimal valueTotal)
        {
            return (valueTotal * (Percentage / 100)) == Value;
        }

        public bool ValidateNotEqualTotal(decimal valueTotal)
        {
            return Value != valueTotal;
        }
    }
}
