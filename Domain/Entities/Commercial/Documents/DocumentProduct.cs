using Domain.Interfaces.Entities;
using Domain.Utilities.Entities;

namespace Domain.Entities.Commercial.Documents
{
    public abstract class DocumentProduct : EntityWithoutDescription, ICopyable<DocumentProduct>, IEntityChild
    {
        public long DocumentId { get; private set; }

        public Document? Document { get; private set; }

        public long ProductId { get; private set; }

        public Product? Product { get; private set; }

        public decimal Quantity { get; private set; }

        public decimal ValueUnit { get; private set; }

        public decimal ValueTotal { get; private set; }

        protected override string GetDescription()
        {
            return $"{nameof(Product)}: {ProductId} of {nameof(Document)}: {DocumentId}";
        }

        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        protected DocumentProduct() { }

        public DocumentProduct(long documentId, long productId, decimal quantity, decimal valueUnit)
        {
            Fill(documentId, productId, quantity, valueUnit);
        }

        public void Copy(DocumentProduct product)
        {
            Fill(product.DocumentId, product.ProductId, product.Quantity, product.ValueUnit);
        }

        public void SetFatherEntityId(long id)
        {
            SetDocumentId(id);
        }

        private void Fill(long documentId, long productId, decimal quantity, decimal valueUnit)
        {
            SetDocumentId(documentId);

            SetProductId(productId);

            SetValues(quantity, valueUnit);
        }

        private void SetValues(decimal quantity, decimal valueUnit)
        {
            Validator.ValidateValue(quantity, nameof(Quantity));
            Quantity = quantity;

            Validator.ValidateValue(valueUnit, nameof(ValueUnit));
            ValueUnit = valueUnit;

            ValueTotal = Quantity * ValueUnit;
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
