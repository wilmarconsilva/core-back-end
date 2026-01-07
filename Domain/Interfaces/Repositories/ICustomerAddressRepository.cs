using Domain.Entities.Persons.Customers;
namespace Domain.Interfaces.Repositories
{
    public interface ICustomerAddressRepository : IRepositoryCrud<CustomerAddress>
    {
        Task<IReadOnlyList<long>> GetIdsByCustomerAsync(IEnumerable<long> customerIds, CancellationToken cancellationToken);
    }
}