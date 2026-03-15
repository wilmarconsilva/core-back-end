using Domain.Entities.Commercial.Documents.Orders;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistece;

namespace Infrastructure.Repositories.Commercial.Documents.Orders
{
    public class OrderRepository : DocumentoRepository<Order, OrderProduct>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }
    }
}
