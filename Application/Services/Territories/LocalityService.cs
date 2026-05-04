using Application.DataTransferObjects.Territories;
using Application.DataTransferObjects.Territories.Listings;
using Application.Interfaces.Queries.Territories;
using Application.Interfaces.Services.Territories;
using Domain.Entities.Territories;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;

namespace Application.Services.Territories
{
    public class LocalityService : CrudService<Locality, LocalitySaveDto, LocalityDetailDto, LocalityListing, LocalityListingFilter>, ILocalityService
    {
        public LocalityService(ILocalityRepository repository, ILocalityQuery query, IUnitOfWork unitOfWork) : base(repository, query, unitOfWork)
        {
        }

        protected override Locality CreateEntity(LocalitySaveDto dto)
        {
            return new Locality(dto.Description, dto.StateId);
        }

        protected override void UpdateEntity(LocalitySaveDto dto, Locality entity)
        {
            entity.Fill(dto.Description, dto.StateId);
        }

        protected override Task<bool> EntitiesCanBeDeleted(IEnumerable<long> ids)
        {
            return Task.FromResult(true);
        }
    }
}
