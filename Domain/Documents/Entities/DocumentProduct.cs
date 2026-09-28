using Domain.Exceptions;
using Domain.Shared.Interfaces;
using Domain.Shared.Utilities;
using Domain.Shared.Entities;
using Domain.Products.Entities;

namespace Domain.Documents.Entities
{
    public abstract class DocumentProduct : EntityWithoutDescription, ICopyable<DocumentProduct>, IEntityChild
    {
        public long DocumentId { get; private set; }

        public long ProductId { get; private set; }

        public Product? Product { get; private init; }

        public decimal Quantity { get; private set; }

        public decimal ValueUnit { get; private set; }

        public decimal ValueTotal { get; private set; }

        protected override string GetDescription()
        {
            return $"Produto: {ProductId} do documento: {DocumentId}";
        }

        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        protected DocumentProduct() { }

        protected DocumentProduct(long productId, decimal quantity, decimal valueUnit, decimal valueTotal)
        {
            Fill(productId, quantity, valueUnit, valueTotal);
        }

        protected DocumentProduct(long documentId, long productId, decimal quantity, decimal valueUnit, decimal valueTotal)
        {
            SetDocumentId(documentId);

            Fill(productId, quantity, valueUnit, valueTotal);
        }

        public void Copy(DocumentProduct product)
        {
            Fill(product.ProductId, product.Quantity, product.ValueUnit, product.ValueTotal);
        }

        public void SetFatherEntityId(long id)
        {
            SetDocumentId(id);
        }

        private void Fill(long productId, decimal quantity, decimal valueUnit, decimal valueTotal)
        {
            SetProductId(productId);

            SetValues(quantity, valueUnit, valueTotal);
        }

        private void SetValues(decimal quantity, decimal valueUnit, decimal valueTotal)
        {
            ValueValidatorUtility.ValidateValue(quantity, nameof(Quantity));
            Quantity = quantity;

            ValueValidatorUtility.ValidateValue(valueUnit, nameof(ValueUnit));
            ValueUnit = valueUnit;

            ValueValidatorUtility.ValidateValue(valueTotal, nameof(ValueTotal));
            ValueTotal = valueTotal;

            if ((quantity * valueUnit) != valueTotal)
                throw new BusinessRuleException($"O valor total do produto de código: {ProductId} não corresponde com a sua quantidade multiplicada pelo seu valor unitário.");
        }

        private void SetProductId(long productId)
        {
            ValueValidatorUtility.ValidateValue(productId, nameof(ProductId));
            ProductId = productId;
        }

        private void SetDocumentId(long documentId)
        {
            ValueValidatorUtility.ValidateValue(documentId, nameof(DocumentId));
            DocumentId = documentId;
        }
    }
}
