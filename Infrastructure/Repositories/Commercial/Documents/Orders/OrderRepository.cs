using Domain.Documents.Orders.Entities;
using Domain.Documents.Orders.Repositories.Interfaces;
using Infrastructure.Shared.Persistance;

namespace Infrastructure.Repositories.Commercial.Documents.Orders
{
    public class OrderRepository : DocumentoRepository<Order, OrderProduct>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }
    }
}
