using Application.Interfaces.Queries.Territories;
using Application.Interfaces.Services.Territories;
using Application.Shared.Services;
using Application.Territories.Localities.DataTransferObjects;
using Application.Territories.Localities.DataTransferObjects.Listings;
using Domain.Exceptions;
using Domain.Shared.Persistance.Interfaces;
using Domain.Territories.Localities.Entities;
using Domain.Territories.Localities.Repositories.Interfaces;

namespace Application.Territories.Localities.Services
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
