using Application.Shared.Services;
using Application.Territories.Localities.DataTransferObjects;
using Application.Territories.Localities.DataTransferObjects.Listings;
using Application.Territories.Localities.DataTransferObjects.Synchronizations;
using Application.Territories.Localities.Queries.Interfaces;
using Application.Territories.Localities.Services.Interfaces;
using Domain.Shared.Exceptions;
using Domain.Shared.Persistance.Interfaces;
using Domain.Territories.Localities.Entities;
using Domain.Territories.Localities.Repositories.Interfaces;

namespace Application.Territories.Localities.Services
{
    public class LocalityService : CrudService<Locality, LocalitySaveDto, LocalityDetailDto, LocalityListing, LocalityListingFilter>, ILocalityService
    {
        private readonly ILocalityRepository _repository;
        private readonly ILocalityQuery _localityQuery;

        public LocalityService(ILocalityRepository repository, ILocalityQuery query, IUnitOfWork unitOfWork) : base(repository, query, unitOfWork)
        {
            _repository = repository;
            _localityQuery = query;
        }

        public Task<List<LocalitySynchronizationDto>> GetSynchronizationAsync(LocalityListingFilter filter, CancellationToken cancellationToken)
        {
            return _localityQuery.GetSynchronization(filter, cancellationToken);
        }

        protected override Locality CreateEntity(LocalitySaveDto dto)
        {
            throw new BusinessRuleException("Operação não permitida. Localidades não podem ser criadas.");
        }

        protected override void UpdateEntity(LocalitySaveDto dto, Locality entity)
        {
            throw new BusinessRuleException("Operação não permitida. Localidades não podem ser alteradas.");
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
