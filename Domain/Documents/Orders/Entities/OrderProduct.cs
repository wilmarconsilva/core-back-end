using Domain.Documents.Entities;

namespace Domain.Documents.Orders.Entities
{
    public class OrderProduct : DocumentProduct
    {
        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        public OrderProduct() { }

        public OrderProduct(long productId, decimal quantity, decimal valueUnit, decimal valueTotal)
            : base(productId, quantity, valueUnit, valueTotal)
        {
        }
    }
}