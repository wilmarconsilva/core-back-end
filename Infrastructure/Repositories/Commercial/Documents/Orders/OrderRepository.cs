using Domain.Entities.Commercial.Documents.Orders;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistece;

namespace Infrastructure.Repositories.Commercial.Documents.Orders
{
    public class OrderRepository : RepositoryCrud<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }
    }
}
