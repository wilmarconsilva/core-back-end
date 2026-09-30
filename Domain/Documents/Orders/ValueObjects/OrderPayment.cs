using Domain.Documents.ValueObjects.Payments;
using Domain.Documents.ValueObjects.Payments.Enums;
using Domain.Documents.ValueObjects.Payments.Installments;
using Domain.Shared.Utilities;

namespace Domain.Documents.Orders.ValueObjects
{
    public class OrderPayment : Payment
    {
        public PaymentMethod Method { get; private init; }

        public OrderPayment(PaymentMethod method, decimal discountValue, Installment? installment)
            : base(discountValue, installment)
        {
            ValueValidatorUtility.ValidateEnum(method, $"O método de pagamento");

            Method = method;
        }

        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        public OrderPayment() { }
    }
}