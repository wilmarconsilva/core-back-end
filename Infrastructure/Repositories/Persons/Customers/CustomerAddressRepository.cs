using Domain.Entities.Persons.Customers;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistece;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Persons.Customers
{
    public class CustomerAddressRepository : RepositoryCrud<CustomerAddress>, ICustomerAddressRepository
    {
        private readonly DbSet<CustomerAddress> _dbSet;

        public CustomerAddressRepository(AppDbContext context) : base(context)
        {
            _dbSet = context.Set<CustomerAddress>();
        }

        public async Task<IReadOnlyList<long>> GetIdsByCustomerAsync(IEnumerable<long> customerIds, CancellationToken cancellationToken)
        {
            return await _dbSet.Where(ca => customerIds.Contains(ca.Id)).Select(ca => ca.Id).ToListAsync(cancellationToken);
        }
    }
}