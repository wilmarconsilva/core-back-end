using Application.Documents.Budgets.DataTransferObjects;
using Application.Documents.Budgets.Mappings.Details;
using Application.Documents.Orders.DataTransferObjects;
using Application.Documents.Orders.Mappings.Details;
using Application.Persons.Customers.Mappings.Details;
using Application.Products.Mappings;
using Application.Territories.Localities.Mappings;
using Application.UnitTests.Builders;
using Application.UnitTests.Fixtures;
using AutoMapper.Internal;
using Domain.Documents.Budgets.Entities;
using Domain.Documents.Orders.Entities;
using Domain.Shared.Exceptions;
using Domain.UnitTests.Builders;

namespace Application.UnitTests.Shared.Mappings
{
    public class MappingConfigurationTests : IClassFixture<MapperFixture>
    {
        private readonly MapperFixture _fixture;

        public MappingConfigurationTests(MapperFixture fixture)
        {
            _fixture = fixture;
        }

        /// <summary>
        /// Profiles de leitura (Entity → DetailDto) devem preencher todo o DTO devolvido pela API.
        /// Os de escrita (SaveDto → Entity) ficam de fora: a entidade calcula/valida seus membros e os demais são navegações do EF.
        /// </summary>
        [Theory]
        [InlineData(typeof(BudgetDetailProfile))]
        [InlineData(typeof(BudgetProductDetailProfile))]
        [InlineData(typeof(OrderDetailProfile))]
        [InlineData(typeof(OrderProductDetailProfile))]
        [InlineData(typeof(CustomerDetailProfile))]
        [InlineData(typeof(CustomerAddressDetailProfile))]
        [InlineData(typeof(ProductDetailProfile))]
        [InlineData(typeof(LocalityDetailProfile))]
        public void DetailProfile_AllDestinationMembers_AreMapped(Type profileType)
        {
            _fixture.Configuration.Internal().AssertConfigurationIsValid(profileType.FullName!);
        }

        [Fact]
        public void BudgetDetailProfile_Seller_MapsToUserId()
        {
            Budget budget = DocumentBuilder.Budget(sellerId: 3);

            BudgetDetailDto detail = _fixture.Mapper.Map<BudgetDetailDto>(budget);

            detail.SellerId.ShouldBe(3);
        }

        [Fact]
        public void BudgetProductSaveDto_ValidValues_MapsToBudgetProduct()
        {
            BudgetProduct product = _fixture.Mapper.Map<BudgetProduct>(DocumentSaveDtoBuilder.BudgetProduct(productId: 10, quantity: 2, valueUnit: 50, id: 3));

            product.Id.ShouldBe(3);
            product.ProductId.ShouldBe(10);
            product.Quantity.ShouldBe(2);
            product.ValueUnit.ShouldBe(50);
            product.ValueTotal.ShouldBe(100);
        }

        [Fact]
        public void BudgetProductSaveDto_InvalidValues_EnforcesDomainInvariants()
        {
            BudgetProductSaveDto dto = DocumentSaveDtoBuilder.BudgetProduct();
            dto.ValueUnit = 0;

            Exception exception = Should.Throw<Exception>(() => _fixture.Mapper.Map<BudgetProduct>(dto));

            exception.GetBaseException().ShouldBeOfType<BusinessRuleException>();
        }

        [Fact]
        public void OrderProductSaveDto_InvalidValues_EnforcesDomainInvariants()
        {
            OrderProductSaveDto dto = DocumentSaveDtoBuilder.OrderProduct();
            dto.Quantity = 0;

            Exception exception = Should.Throw<Exception>(() => _fixture.Mapper.Map<OrderProduct>(dto));

            exception.GetBaseException().ShouldBeOfType<BusinessRuleException>();
        }
    }
}
