using Domain.Utilities.Entities;

namespace Domain.ValueObjects.Commercial.Documents.Payments.Installments
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
            Validator.ValidateValue(quantity, $"A quantidade de parcelas");

            Quantity = quantity;
            DownPayment = downPayment;

            if (downPayment != null)
                HasDownPayment = true;
        }

        public void SetValue(decimal valueTotal)
        {
            decimal value = valueTotal / Quantity;
            Validator.ValidateValue(value, "O valor da(s) parcela(s)");
            Value = value;
        }

        public bool ValidateInstallmentTotal(decimal valueTotal)
        {
            decimal downPaymentValue = DownPayment?.Value ?? 0;
            return (Value * Quantity) - downPaymentValue == (valueTotal - downPaymentValue);
        }

        public bool ValidateDownPaymentPercentage(decimal valueTotal)
        {
            if (DownPayment == null)
                return true;

            return DownPayment.ValidatePercentageCorrespondsToValue(valueTotal);
        }

        public bool ValidateDownPaymentValueLessThanTotal(decimal valueTotal)
        {
            if (DownPayment == null)
                return true;

            return DownPayment.ValidateValueLessThanTotal(valueTotal);
        }
    }
}
