using Domain.Documents.Orders.Entities;
using Infrastructure.Documents.Persistance.Mappings;

namespace Infrastructure.Documents.Orders.Persistance.Mappings
{
    public class OrderProductMapping : DocumentProductMapping<OrderProduct>
    {
        protected override string GetTableName()
        {
            return "orders_products";
        }
    }
}
