using Application.DataTransferObjects.Territories;
using Application.DataTransferObjects.Territories.Listings;
using Application.Interfaces.Queries.Territories;
using Application.Interfaces.Services.Territories;
using Domain.Entities.Territories;
using Domain.Exceptions;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;

namespace Application.Services.Territories
{
    public class LocalityService : CrudService<Locality, LocalitySaveDto, LocalityDetailDto, LocalityListing, LocalityListingFilter>, ILocalityService
    {
        private readonly ILocalityRepository _repository;

        public LocalityService(ILocalityRepository repository, ILocalityQuery query, IUnitOfWork unitOfWork) : base(repository, query, unitOfWork)
        {
            _repository = repository;
        }

        protected override Locality CreateEntity(LocalitySaveDto dto)
        {
            throw new BusinessRuleException("Operação não permitida. Localidades não podem ser criadas.");
            //return new Locality(dto.Description, dto.StateId);
        }

        protected override void UpdateEntity(LocalitySaveDto dto, Locality entity)
        {
            throw new BusinessRuleException("Operação não permitida. Localidades não podem ser alteradas.");
            //entity.Fill(dto.Description, dto.StateId);
        }

        protected override Task<bool> EntitiesCanBeDeleted(IEnumerable<long> ids)
        {
            return Task.FromResult(false);
        }

        public async Task<long> GetQuantityAsync(CancellationToken cancellationToken)
        {
            return await _repository.GetQuantityAsync(cancellationToken);
        }
    }
}
