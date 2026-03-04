using Domain.Exceptions;
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

        public DownPayment(decimal value, decimal percentage, PaymentMethod method, bool hasDownPayment)
        {
            ValidateValue(value, nameof(Value));
            ValidateValue(percentage, nameof(Percentage));
            ValidateEnum(method, nameof(Method));

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

        private static void ValidateValue(decimal value, string propertyName)
        {
            if (value <= 0)
                throw new BusinessRuleException($"{propertyName} deve ser maior que zero.");
        }

        private static void ValidateEnum<TEnum>(TEnum enumValue, string propertyName) where TEnum : Enum
        {
            if (!Enum.IsDefined(typeof(TEnum), enumValue))
                throw new BusinessRuleException($"{propertyName} contém um valor inválido.");
        }
    }
}
