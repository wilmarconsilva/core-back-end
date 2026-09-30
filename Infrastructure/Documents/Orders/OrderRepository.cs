using Domain.Documents.Orders.Entities;
using Domain.Documents.Orders.ValueObjects;
using Domain.Documents.Orders.Repositories.Interfaces;
using Infrastructure.Shared.Persistance;

namespace Infrastructure.Documents.Orders
{
    public class OrderRepository : DocumentoRepository<Order, OrderProduct, OrderPayment>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }
    }
}
