using Domain.Documents.Interfaces;
using Domain.Documents.ValueObjects.Payments;
using Domain.Persons.Customers.Entities;
using Domain.Persons.Users.Entities;
using Domain.Shared.Entities;
using Domain.Shared.Utilities;
using Domain.Shared.Exceptions;

namespace Domain.Documents.Entities
{
    public abstract class Document<TProduct, TPayment> : EntityWithoutDescription, IDocument where TProduct : DocumentProduct where TPayment : Payment
    {
        private TPayment _payment = null!;
        private readonly List<TProduct> _products = new();

        public DateTime CreationDateTime { get; private init; }

        public DateTime LastUpdateDateTime { get; private set; }

        public long UserId { get; private set; }

        public User? User { get; private set; }

        public long CustomerId { get; private set; }

        public Customer? Customer { get; private set; }

        public decimal ValueSubtotal { get; private set; }

        public decimal ValueTotal { get; private set; }

        public TPayment Payment => _payment;

        public string? Observations { get; set; }

        public byte[]? CustomerSignatureImage { get; set; }

        public byte[]? PDF { get; set; }

        public IReadOnlyCollection<TProduct> Products => _products;

        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        protected Document() { }

        protected Document(
            long userId,
            DateTime creationDateTime,
            long customerId, TPayment payment,
            string? observations, byte[]? customerSignatureImage,
            byte[]? pdf,
            IReadOnlyCollection<TProduct> products)
        {
            CreationDateTime = creationDateTime;
            Fill(userId, customerId, payment, observations, customerSignatureImage, pdf, products);
        }

        public void Fill(
            long userId,
            long customerId,
            TPayment payment,
            string? observations,
            byte[]? customerSignatureImage,
            byte[]? pdf,
            IReadOnlyCollection<TProduct> products)
        {
            LastUpdateDateTime = DateTime.UtcNow;
            SetUserId(userId);
            SetCustomerId(customerId);
            _payment = payment;
            PDF = pdf;

            if (products == null || !products.Any())
                throw new BusinessRuleException($"O Orçamento: {this.Description} deve possuir ao menos um produto.");

            EntityCollectionUtility.Sync(_products, products, Id);

            SetValues();
            Observations = observations;
            CustomerSignatureImage = customerSignatureImage;
            PDF = pdf;
        }

        protected override string GetDescription()
        {
            return Id.ToString();
        }

        private void SetUserId(long userId)
        {
            ValueValidatorUtility.ValidateValue(userId, nameof(UserId));
            UserId = userId;
        }

        private void SetCustomerId(long customerId)
        {
            ValueValidatorUtility.ValidateValue(customerId, nameof(CustomerId));
            CustomerId = customerId;
        }

        private void SetValues()
        {
            SetValueSubtotal();

            SetValueTotal();

            if (Payment.Installment != null && Payment.HasInstallment)
            {
                Payment.Installment.SetValue(ValueTotal);
            }
        }

        private void SetValueTotal()
        {
            decimal valueTotal = ValueSubtotal - Payment.DiscountValue;
            ValueValidatorUtility.ValidateValue(valueTotal, nameof(ValueTotal));
            ValueTotal = valueTotal;
        }

        private void SetValueSubtotal()
        {
            decimal valueSubtotal = Products.Sum(p => p.ValueTotal);
            ValueValidatorUtility.ValidateValue(valueSubtotal, nameof(ValueSubtotal));
            ValueSubtotal = valueSubtotal;
        }
    }
}