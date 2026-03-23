using Domain.Entities.Territories;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistece;

namespace Infrastructure.Repositories.Territories
{
    public class LocalityRepository(AppDbContext context) : RepositoryCrud<Locality>(context), ILocalityRepository
    {

    }
}
