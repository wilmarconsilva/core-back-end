using Domain.Entities.Commercial.Documents.Orders;

namespace Infrastructure.Persistence.Mappings.Commercial.Documents.Orders
{
    public class OrderProductMapping : DocumentProductMapping<OrderProduct>
    {
        protected override string GetTableName()
        {
            return "orders_products";
        }
    }
}
