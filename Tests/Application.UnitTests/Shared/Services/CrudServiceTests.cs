using Application.Products.DataTransferObjects;
using Application.Products.DataTransferObjects.Listings;
using Application.Products.Services;
using Application.Shared.Queries.Interfaces;
using Domain.Products.Entities;
using Domain.Products.Enums;
using Domain.Products.Repositories.Interfaces;
using Domain.Shared.Entities;
using Domain.Shared.Exceptions;
using Domain.Shared.Persistance.Interfaces;

namespace Application.UnitTests.Shared.Services
{
    /// <summary>
    /// Fluxo genérico do <see cref="Application.Shared.Services.CrudService{TEntity, TSaveDto, TDetailDto, TListing, TFilter}"/>,
    /// exercitado pelo <see cref="ProductService"/>, o service concreto mais simples.
    /// </summary>
    public class CrudServiceTests
    {
        private readonly IProductRepository _repository = Substitute.For<IProductRepository>();
        private readonly IProductQuery _query = Substitute.For<IProductQuery>();
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly ProductService _service;

        public CrudServiceTests()
        {
            _service = new ProductService(_repository, _query, _unitOfWork);
        }

        private static ProductSaveDto CreateDto(string description = "Cimento", long id = 0)
        {
            return new ProductSaveDto { Id = id, Description = description, MeasureUnit = MeasureUnit.Kilograms };
        }

        [Fact]
        public async Task CreateAsync_EmptyDtos_ThrowsBusinessRuleException()
        {
            BusinessRuleException exception = await Should.ThrowAsync<BusinessRuleException>(async () => await _service.CreateAsync([], TestContext.Current.CancellationToken));

            exception.Message.ShouldBe("Nenhuma informação foi fornecida para executar a operação.");
        }

        [Fact]
        public async Task CreateAsync_NullDtos_ThrowsBusinessRuleException()
        {
            await Should.ThrowAsync<BusinessRuleException>(async () => await _service.CreateAsync(null!, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task CreateAsync_ValidDto_SavesNewEntityInsideTransaction()
        {
            IEnumerable<Entity> entities = await _service.CreateAsync([CreateDto("Cimento")], TestContext.Current.CancellationToken);

            entities.ShouldHaveSingleItem().Description.ShouldBe("Cimento");
            await _repository.DidNotReceive().GetByIdsAsync(Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>());
            Received.InOrder(() =>
            {
                _unitOfWork.BeginTransactionAsync(Arg.Any<CancellationToken>());
                _repository.SaveAsync(Arg.Is<IEnumerable<Product>>(p => p.Single().Description == "Cimento"), Arg.Any<CancellationToken>());
                _unitOfWork.CommitAsync(Arg.Any<CancellationToken>());
            });
        }

        [Fact]
        public async Task UpdateAsync_ExistingId_UpdatesLoadedEntity()
        {
            Product existing = new("Cimento", MeasureUnit.Kilograms) { Id = 5 };
            _repository.GetByIdsAsync(Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns([existing]);

            await _service.UpdateAsync(5, CreateDto("Areia"), TestContext.Current.CancellationToken);

            existing.Description.ShouldBe("Areia");
            await _repository.Received(1).GetByIdsAsync(Arg.Is<IEnumerable<long>>(ids => ids.SequenceEqual(new long[] { 5 })), Arg.Any<CancellationToken>());
            await _repository.Received(1).SaveAsync(Arg.Is<IEnumerable<Product>>(p => p.Single() == existing), Arg.Any<CancellationToken>());
            await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdateAsync_UnknownId_ThrowsNotFoundExceptionWithoutTransaction()
        {
            _repository.GetByIdsAsync(Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns(new List<Product>());

            NotFoundException exception = await Should.ThrowAsync<NotFoundException>(() => _service.UpdateAsync(5, CreateDto(), TestContext.Current.CancellationToken));

            exception.Message.ShouldBe("Nenhum registro encontrado para o(s) código(s): 5.");
            await _unitOfWork.DidNotReceive().BeginTransactionAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdateAsync_MixedDtos_UpdatesExistingAndCreatesNew()
        {
            Product existing = new("Cimento", MeasureUnit.Kilograms) { Id = 5 };
            _repository.GetByIdsAsync(Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns([existing]);

            await _service.UpdateAsync([CreateDto("Areia", id: 5), CreateDto("Brita")], TestContext.Current.CancellationToken);

            await _repository.Received(1).SaveAsync(
                Arg.Is<IEnumerable<Product>>(p => p.Count() == 2 && p.Any(e => e.Id == 5 && e.Description == "Areia") && p.Any(e => e.Id == 0 && e.Description == "Brita")),
                Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeleteAsync_UnknownId_ThrowsNotFoundExceptionWithoutTransaction()
        {
            _repository.GetIdsAsync(Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns(new List<long> { 1 });

            NotFoundException exception = await Should.ThrowAsync<NotFoundException>(() => _service.DeleteAsync([1, 2], TestContext.Current.CancellationToken));

            exception.Message.ShouldBe("Nenhum registro encontrado para o(s) código(s): 2.");
            await _unitOfWork.DidNotReceive().BeginTransactionAsync(Arg.Any<CancellationToken>());
            await _repository.DidNotReceive().DeleteAsync(Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task DeleteAsync_ExistingId_DeletesInsideTransaction()
        {
            _repository.GetIdsAsync(Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns(new List<long> { 5 });

            await _service.DeleteAsync(5, TestContext.Current.CancellationToken);

            Received.InOrder(() =>
            {
                _unitOfWork.BeginTransactionAsync(Arg.Any<CancellationToken>());
                _repository.DeleteAsync(Arg.Is<IEnumerable<long>>(ids => ids.SequenceEqual(new long[] { 5 })), Arg.Any<CancellationToken>());
                _unitOfWork.CommitAsync(Arg.Any<CancellationToken>());
            });
        }

        [Fact]
        public async Task GetByIdAsync_UnknownId_ThrowsNotFoundException()
        {
            _query.GetDetailAsync(5, Arg.Any<CancellationToken>()).Returns((ProductDetailDto?)null);

            await Should.ThrowAsync<NotFoundException>(() => _service.GetByIdAsync(5, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsDetail()
        {
            ProductDetailDto detail = new() { Description = "Cimento", MeasureUnit = MeasureUnit.Kilograms };
            _query.GetDetailAsync(5, Arg.Any<CancellationToken>()).Returns(detail);

            ProductDetailDto result = await _service.GetByIdAsync(5, TestContext.Current.CancellationToken);

            result.ShouldBeSameAs(detail);
        }

        [Fact]
        public async Task GetAllAsync_Filter_DelegatesToQuery()
        {
            ProductListingFilter filter = new() { Description = "Cim%" };
            List<ProductListing> listing = [];
            _query.GetAll(filter, Arg.Any<CancellationToken>()).Returns(listing);

            List<ProductListing> result = await _service.GetAllAsync(filter, TestContext.Current.CancellationToken);

            result.ShouldBeSameAs(listing);
        }
    }
}
