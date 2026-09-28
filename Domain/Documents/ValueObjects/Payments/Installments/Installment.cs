using Domain.Shared.Utilities;

namespace Domain.Documents.ValueObjects.Payments.Installments
{
    public class Installment
    {
        public decimal Value { get; private set; }

        public int Quantity { get; private init; }

        public bool HasDownPayment { get; private init; }

        public DownPayment? DownPayment { get; private init; }

        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        private Installment() { }

        public Installment(int quantity, DownPayment? downPayment)
        {
            ValueValidatorUtility.ValidateValue(quantity, $"A quantidade de parcelas");

            Quantity = quantity;
            DownPayment = downPayment;

            if (downPayment != null)
                HasDownPayment = true;
        }

        public void SetValue(decimal valueTotal)
        {
            if (DownPayment != null)
                valueTotal -= DownPayment.Value;

            decimal value = valueTotal / Quantity;
            ValueValidatorUtility.ValidateValue(value, "O valor da(s) parcela(s)");
            Value = value;
        }

        public bool ValidateInstallmentValueQuantity(decimal valueTotal)
        {
            decimal downPaymentValue = DownPayment?.Value ?? 0;
            return (Value * Quantity) == (valueTotal - downPaymentValue);
        }

        public bool ValidateDownPaymentPercentage(decimal valueTotal)
        {
            if (DownPayment == null)
                return true;

            return DownPayment.ValidatePercentage(valueTotal);
        }

        public bool ValidateDownPaymentValue(decimal valueTotal)
        {
            if (DownPayment == null)
                return true;

            return DownPayment.ValidateValue(valueTotal);
        }
    }
}
