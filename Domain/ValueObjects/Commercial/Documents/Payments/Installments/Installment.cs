using Domain.Exceptions;

namespace Domain.ValueObjects.Commercial.Documents.Payments.Installments
{
    public class Installment
    {
        public decimal Value { get; private init; }

        public int Quantity { get; private init; }

        public bool HasDownPayment { get; private init; }

        public DownPayment? DownPayment { get; private init; }

        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        private Installment() { }

        public Installment(decimal value, int quantity, DownPayment? downPayment)
        {
            ValidateValue(value, nameof(Value));
            ValidateValue(quantity, nameof(Quantity));

            Value = value;
            Quantity = quantity;
            DownPayment = downPayment;

            if (downPayment != null)
                HasDownPayment = true;
        }

        public bool ValidateInstallmentTotal(decimal valueTotal)
        {
            decimal downPaymentValue = DownPayment?.Value ?? 0;
            return (Value * Quantity) == (valueTotal - downPaymentValue);
        }

        public bool ValidateDownPaymentPercentage(decimal valueTotal)
        {
            if (DownPayment == null)
                return true;

            return DownPayment.ValidatePercentageCorrespondsToValue(valueTotal);
        }

        public bool ValidateDownPaymentNotEqualTotal(decimal valueTotal)
        {
            if (DownPayment == null)
                return true;

            return DownPayment.ValidateNotEqualTotal(valueTotal);
        }

        private static void ValidateValue(decimal value, string propertyName)
        {
            if (value <= 0)
                throw new BusinessRuleException($"{propertyName} deve ser maior que zero.");
        }

        private static void ValidateValue(int value, string propertyName)
        {
            if (value <= 0)
                throw new BusinessRuleException($"{propertyName} deve ser maior que zero.");
        }
    }
}
