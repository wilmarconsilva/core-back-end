using Domain.Exceptions;
using Domain.ValueObjects.CommercialDocuments.Payments.Installments;
using Domain.ValueObjects.Enums;

namespace Domain.ValueObjects.CommercialDocuments.Payments
{
    public class Payment
    {
        public PaymentMethod Method { get; private init; }

        public decimal DiscountValue { get; private init; }

        public bool HasInstallment { get; private init; }

        public Installment? Installment { get; private init; }

        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        private Payment() { }

        public Payment(PaymentMethod method, decimal discountValue, Installment? installments)
        {
            ValidateEnum(method, nameof(Method));
            ValidateValue(discountValue, nameof(DiscountValue));

            Method = method;
            DiscountValue = discountValue;
            Installment = installments;

            if (installments != null)
                HasInstallment = true;
        }

        private static void ValidateEnum<TEnum>(TEnum value, string propertyName) where TEnum : Enum
        {
            if (!Enum.IsDefined(typeof(TEnum), value))
                throw new BusinessRuleException($"{propertyName} contém um valor inválido.");
        }

        private static void ValidateValue(decimal value, string propertyName)
        {
            if (value < 0)
                throw new BusinessRuleException($"{propertyName} não pode ser negativo.");
        }
    }
}
