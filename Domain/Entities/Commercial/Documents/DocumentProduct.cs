using Domain.Interfaces.Entities;
using Domain.Utilities.Entities;
using Domain.Exceptions;

namespace Domain.Entities.Commercial.Documents
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

        public DocumentProduct(long documentId, long productId, decimal quantity, decimal valueUnit, decimal valueTotal)
        {
            Fill(documentId, productId, quantity, valueUnit, valueTotal);
        }

        public void Copy(DocumentProduct product)
        {
            Fill(product.DocumentId, product.ProductId, product.Quantity, product.ValueUnit, product.ValueTotal);
        }

        public void SetFatherEntityId(long id)
        {
            SetDocumentId(id);
        }

        private void Fill(long documentId, long productId, decimal quantity, decimal valueUnit, decimal valueTotal)
        {
            SetDocumentId(documentId);

            SetProductId(productId);

            SetValues(quantity, valueUnit, valueTotal);
        }

        private void SetValues(decimal quantity, decimal valueUnit, decimal valueTotal)
        {
            Validator.ValidateValue(quantity, nameof(Quantity));
            Quantity = quantity;

            Validator.ValidateValue(valueUnit, nameof(ValueUnit));
            ValueUnit = valueUnit;

            Validator.ValidateValue(valueTotal, nameof(ValueTotal));
            ValueTotal = valueTotal;

            if ((quantity * valueUnit) != valueTotal)
                throw new BusinessRuleException($"O valor total do produto de código: {ProductId} não corresponde com a sua quantidade multiplicada pelo seu valor unitário.");
        }

        private void SetProductId(long productId)
        {
            Validator.ValidateValue(productId, nameof(ProductId));
            ProductId = productId;
        }

        private void SetDocumentId(long documentId)
        {
            Validator.ValidateValue(documentId, nameof(DocumentId));
            DocumentId = documentId;
        }
    }
}
