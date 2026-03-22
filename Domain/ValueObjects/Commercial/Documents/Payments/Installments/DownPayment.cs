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
            Validator.ValidateValue(value, "O valor da entrada");
            Validator.ValidateValue(percentage, "A porcentagem da entrada");
            Validator.ValidateEnum(method, "O método de pagamento da entrada");

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
