using Domain.Entities.Persons;
using Domain.Entities.Persons.Customers;
using Domain.Exceptions;
using Domain.Interfaces.Entities.Commercial;
using Domain.Utilities.Entities;
using Domain.ValueObjects.Commercial.Documents.Payments;

namespace Domain.Entities.Commercial.Documents
{
    public abstract class Document<TProduct> : EntityWithoutDescription, IDocument where TProduct : DocumentProduct
    {
        private Payment _payment = null!;
        private readonly List<TProduct> _products = new();

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

        public byte[]? CustomerSignatureImage { get; set; }

        public byte[]? PDF { get; set; }

        public IReadOnlyCollection<TProduct> Products => _products;

        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        protected Document() { }

        protected Document(long userId, DateTime creationDateTime, long customerId, Payment payment, string? observations, byte[]? customerSignatureImage, byte[]? pdf, IReadOnlyCollection<TProduct> products)
        {
            CreationDateTime = creationDateTime;
            Fill(userId, customerId, payment, observations, customerSignatureImage, pdf, products);
        }

        public void Fill(long userId, long customerId, Payment payment, string? observations, byte[]? customerSignatureImage, byte[]? pdf, IReadOnlyCollection<TProduct> products)
        {
            LastUpdateDateTime = DateTime.UtcNow;
            SetUserId(userId);
            SetCustomerId(customerId);
            _payment = payment;
            PDF = pdf;

            if (products == null || !products.Any())
                throw new BusinessRuleException($"O Orçamento: {this.Description} deve possuir ao menos um produto.");

            EntityCollection.Sync(_products, products, Id);

            SetValues();
            Observations = observations;
            CustomerSignatureImage = customerSignatureImage;
        }

        protected override string GetDescription()
        {
            return Id.ToString();
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

            _payment.Installment?.SetValue(ValueTotal);

            ValidateInstallment();
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

        private void ValidateInstallment()
        {
            if (Payment.Installment == null || !Payment.HasInstallment)
                return;

            if (!Payment.Installment.ValidateDownPaymentPercentage(ValueTotal))
                throw new BusinessRuleException("O valor da entrada não corresponde a porcentagem da entrada em relação ao valor total do documento.");

            if (!Payment.Installment.ValidateDownPaymentValue(ValueTotal))
                throw new BusinessRuleException("O valor da entrada não pode ser igual ou maior que o valor total do documento.");

            if (!Payment.Installment.ValidateInstallmentValueQuantity(ValueTotal))
                throw new BusinessRuleException("O valor total da(s) parcela(s) não corresponde ao valor total do documento descontando o valor da entrada.");
        }
    }
}