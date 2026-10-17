using Domain.Documents.Entities;

namespace Domain.Documents.Orders.Entities
{
    public class OrderProduct : DocumentProduct
    {
        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        public OrderProduct() { }

        public OrderProduct(long productId, decimal quantity, decimal valueUnit)
            : base(productId, quantity, valueUnit)
        {
        }
    }
}