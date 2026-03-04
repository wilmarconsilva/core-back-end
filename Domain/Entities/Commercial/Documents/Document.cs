using Domain.Entities.Persons;
using Domain.Entities.Persons.Customers;
using Domain.Exceptions;
using Domain.Utilities.Entities;
using Domain.ValueObjects.Commercial.Documents.Payments;
using Domain.ValueObjects.Commercial.Documents.Payments.Installments;

namespace Domain.Entities.Commercial.Documents
{
    public abstract class Document : EntityWithoutDescription
    {
        private Payment _payment = null!;
        private byte[] _customerSignatureImage = null!;
        private List<DocumentProduct> _products = new();

        public DateTime CreationDateTime { get; private init; }

        public DateTime LastUpdateDateTime { get; private set; }

        public long UserId { get; private set; }

        public User? User { get; private set; }

        public long CustomerId { get; private set; }

        public Customer? Customer { get; private set; }

        public decimal ValueSubtotal { get; private set; }

        public decimal ValueTotal { get; private set; }

        public Payment Payment => _payment;

        public string? Observations { get; set; }

        public byte[] CustomerSignatureImage => _customerSignatureImage;

        public IReadOnlyCollection<DocumentProduct> Products => _products;

        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        protected Document() { }

        protected Document(long userId, long customerId, Payment payment, string? observations, byte[] customerSignatureImage, List<DocumentProduct> products)
        {
            CreationDateTime = DateTime.UtcNow;
            Fill(userId, customerId, payment, observations, customerSignatureImage, products);
        }

        public void Fill(long userId, long customerId, Payment payment, string? observations, byte[] customerSignatureImage, List<DocumentProduct> products)
        {
            LastUpdateDateTime = DateTime.UtcNow;
            SetUserId(userId);
            SetCustomerId(customerId);
            _payment = payment;
            SetValues();
            Observations = observations;
            SetCustomerSignatureImage(customerSignatureImage);

            EntityCollection.Sync(_products, products, Id);
        }

        protected override string GetDescription()
        {
            return Id.ToString();
        }

        private void SetCustomerSignatureImage(byte[] customerSignatureImage)
        {
            Validator.ValidateValue(customerSignatureImage, nameof(CustomerSignatureImage));
            _customerSignatureImage = customerSignatureImage;
        }

        private void SetUserId(long userId)
        {
            Validator.ValidateValue(userId, nameof(UserId));
            UserId = userId;
        }

        private void SetCustomerId(long customerId)
        {
            Validator.ValidateValue(customerId, nameof(CustomerId));
            CustomerId = customerId;
        }

        private void SetValues()
        {
            SetValueSubtotal();

            SetValueTotal();

            ValidateInstallments();
        }

        private void SetValueTotal()
        {
            decimal valueTotal = ValueSubtotal - Payment.DiscountValue;
            Validator.ValidateValue(valueTotal, nameof(ValueTotal));
            ValueTotal = valueTotal;
        }

        private void SetValueSubtotal()
        {
            decimal valueSubtotal = Products.Sum(p => p.ValueTotal);
            Validator.ValidateValue(valueSubtotal, nameof(ValueSubtotal));
            ValueSubtotal = valueSubtotal;
        }

        private void ValidateInstallments()
        {
            if (Payment.Installment == null || !Payment.HasInstallment)
                return;

            if (!Payment.Installment.ValidateInstallmentTotal(ValueTotal))
                throw new BusinessRuleException($"{nameof(Payment.Installment.Value)} não corresponde ao {nameof(ValueTotal)} descontando a {nameof(DownPayment)}.");

            if (!Payment.Installment.ValidateDownPaymentPercentage(ValueTotal))
                throw new BusinessRuleException($"{nameof(DownPayment.Percentage)} não corresponde ao {nameof(DownPayment.Value)}.");

            if (!Payment.Installment.ValidateDownPaymentNotEqualTotal(ValueTotal))
                throw new BusinessRuleException($"{nameof(DownPayment.Value)} não pode ser igual ao {nameof(ValueTotal)}.");
        }
    }
}