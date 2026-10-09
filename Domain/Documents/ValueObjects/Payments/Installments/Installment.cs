using Domain.Shared.Exceptions;
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
            ValueValidatorUtility.ValidatePositive(quantity, "A quantidade de parcelas");

            Quantity = quantity;
            DownPayment = downPayment;

            if (downPayment != null)
                HasDownPayment = true;
        }

        public void SetValue(decimal valueTotal)
        {
            decimal valueWithoutDownPayment = valueTotal - (DownPayment?.Value ?? 0);

            decimal value = Math.Round(valueWithoutDownPayment / Quantity, 2);
            ValueValidatorUtility.ValidateValue(value, "O valor da(s) parcela(s)");
            Value = value;

            Validate(valueTotal);
        }

        private bool ValidateInstallmentValueQuantity(decimal valueTotal)
        {
            decimal downPaymentValue = DownPayment?.Value ?? 0;

            // O valor da parcela é arredondado em centavos, então a soma pode diferir do saldo em até um centavo por parcela.
            decimal tolerance = Quantity * 0.01m;
            return Math.Abs((Value * Quantity) - (valueTotal - downPaymentValue)) < tolerance;
        }

        private bool ValidateDownPaymentPercentage(decimal valueTotal)
        {
            if (DownPayment == null)
                return true;

            return DownPayment.ValidatePercentage(valueTotal);
        }

        private bool ValidateDownPaymentValue(decimal valueTotal)
        {
            if (DownPayment == null)
                return true;

            return DownPayment.ValidateValue(valueTotal);
        }

        private void Validate(decimal valueTotal)
        {
            if (!ValidateDownPaymentPercentage(valueTotal))
                throw new BusinessRuleException("O valor da entrada não corresponde a porcentagem da entrada em relação ao valor total do documento.");

            if (!ValidateDownPaymentValue(valueTotal))
                throw new BusinessRuleException("O valor da entrada não pode ser igual ou maior que o valor total do documento.");

            if (!ValidateInstallmentValueQuantity(valueTotal))
                throw new BusinessRuleException("O valor total da(s) parcela(s) não corresponde ao valor total do documento descontando o valor da entrada.");
        }
    }
}
