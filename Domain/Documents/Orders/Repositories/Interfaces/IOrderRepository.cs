using Domain.Documents.Orders.Entities;
using Domain.Shared.Repositories.Interfaces;

namespace Domain.Documents.Orders.Repositories.Interfaces
{
    public interface IOrderRepository : IRepositoryCrud<Order>
    {

    }
}
