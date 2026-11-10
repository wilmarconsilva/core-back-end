using Domain.Documents.ValueObjects.Payments.Enums;
using Domain.Shared.Utilities;

namespace Domain.Documents.ValueObjects.Payments.Installments
{
    public class DownPayment
    {
        public decimal Value { get; private init; }

        /// <summary>
        /// Percentual da entrada em relação ao valor total do documento, derivado de <see cref="Value"/>
        /// (fonte da verdade) e arredondado em 2 casas decimais. Calculado em <see cref="SetPercentage"/>.
        /// </summary>
        public decimal Percentage { get; private set; }

        public PaymentMethod Method { get; private init; }

        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        private DownPayment() { }

        public DownPayment(decimal value, PaymentMethod method)
        {
            ValueValidatorUtility.ValidatePositive(value, "O valor da entrada");
            ValueValidatorUtility.ValidateEnum(method, "O método de pagamento da entrada");

            Value = Math.Round(value, 2);
            Method = method;
        }

        public void SetPercentage(decimal valueTotal)
        {
            ValueValidatorUtility.ValidatePositive(valueTotal, "O valor total do documento");

            Percentage = Math.Round(Value / valueTotal * 100, 2);
        }

        public bool ValidateValue(decimal valueTotal)
        {
            return Value < valueTotal;
        }
    }
}
