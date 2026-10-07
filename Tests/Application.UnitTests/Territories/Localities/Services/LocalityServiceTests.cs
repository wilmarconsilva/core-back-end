using Application.Territories.Localities.DataTransferObjects;
using Application.Territories.Localities.DataTransferObjects.Listings;
using Application.Territories.Localities.DataTransferObjects.Synchronizations;
using Application.Territories.Localities.Queries.Interfaces;
using Application.Territories.Localities.Services;
using Domain.Shared.Exceptions;
using Domain.Shared.Persistance.Interfaces;
using Domain.Territories.Localities.Entities;
using Domain.Territories.Localities.Repositories.Interfaces;

namespace Application.UnitTests.Territories.Localities.Services
{
    public class LocalityServiceTests
    {
        private readonly ILocalityRepository _repository = Substitute.For<ILocalityRepository>();
        private readonly ILocalityQuery _query = Substitute.For<ILocalityQuery>();
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly LocalityService _service;

        public LocalityServiceTests()
        {
            _service = new LocalityService(_repository, _query, _unitOfWork);
        }

        [Fact]
        public async Task CreateAsync_AnyDto_ThrowsBusinessRuleException()
        {
            BusinessRuleException exception = await Should.ThrowAsync<BusinessRuleException>(
                async () => await _service.CreateAsync([new LocalitySaveDto { Description = "Curitiba" }], TestContext.Current.CancellationToken));

            exception.Message.ShouldBe("Operação não permitida. Localidades não podem ser criadas.");
            await _repository.DidNotReceive().SaveAsync(Arg.Any<IEnumerable<Locality>>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdateAsync_ExistingLocality_ThrowsBusinessRuleException()
        {
            _repository.GetByIdsAsync(Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns([new Locality("Curitiba", null) { Id = 5 }]);

            BusinessRuleException exception = await Should.ThrowAsync<BusinessRuleException>(
                () => _service.UpdateAsync(5, new LocalitySaveDto { Description = "Londrina" }, TestContext.Current.CancellationToken));

            exception.Message.ShouldBe("Operação não permitida. Localidades não podem ser alteradas.");
        }

        [Fact]
        public async Task DeleteAsync_ExistingLocality_ThrowsBusinessRuleExceptionWithoutDeleting()
        {
            _repository.GetIdsAsync(Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns(new List<long> { 5 });

            BusinessRuleException exception = await Should.ThrowAsync<BusinessRuleException>(() => _service.DeleteAsync(5, TestContext.Current.CancellationToken));

            exception.Message.ShouldBe("Os registros informados não podem ser excluídos.");
            await _repository.DidNotReceive().DeleteAsync(Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>());
            await _unitOfWork.DidNotReceive().BeginTransactionAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetQuantityAsync_DelegatesToRepository()
        {
            _repository.GetQuantityAsync(Arg.Any<CancellationToken>()).Returns(5570);

            long quantity = await _service.GetQuantityAsync(TestContext.Current.CancellationToken);

            quantity.ShouldBe(5570);
        }

        [Fact]
        public async Task GetSynchronizationAsync_DelegatesToQuery()
        {
            LocalityListingFilter filter = new();
            List<LocalitySynchronizationDto> synchronization = [];
            _query.GetSynchronization(filter, Arg.Any<CancellationToken>()).Returns(synchronization);

            List<LocalitySynchronizationDto> result = await _service.GetSynchronizationAsync(filter, TestContext.Current.CancellationToken);

            result.ShouldBeSameAs(synchronization);
        }
    }
}
