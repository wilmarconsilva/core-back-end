using Application.Persons.Customers.DataTransferObjects;
using Application.Persons.Customers.DataTransferObjects.CustomerAddresses;
using Application.Persons.Customers.DataTransferObjects.Listings;
using Application.Persons.Customers.DataTransferObjects.Synchronizations;
using Application.Persons.Customers.Queries;
using Application.Persons.Customers.Services;
using Application.UnitTests.Fixtures;
using Domain.Persons.Customers.Entities;
using Domain.Persons.Customers.Repositories.Interfaces;
using Domain.Persons.Enums;
using Domain.Shared.Entities;
using Domain.Shared.Persistance.Interfaces;
using Domain.UnitTests.Builders;

namespace Application.UnitTests.Persons.Customers.Services
{
    public class CustomerServiceTests : IClassFixture<MapperFixture>
    {
        private readonly ICustomerRepository _repository = Substitute.For<ICustomerRepository>();
        private readonly ICustomerQuery _query = Substitute.For<ICustomerQuery>();
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly CustomerService _service;

        public CustomerServiceTests(MapperFixture fixture)
        {
            _service = new CustomerService(_repository, _query, _unitOfWork, fixture.Mapper);
        }

        private static CustomerSaveDto CreateDto(params CustomerAddressSaveDto[] addresses)
        {
            return new CustomerSaveDto
            {
                Name = "João da Silva",
                PersonType = PersonType.Natural,
                Document = CustomerBuilder.ValidCpf,
                Email = "joao@email.com",
                Addresses = addresses.Length > 0 ? [.. addresses] : [CreateAddressDto()]
            };
        }

        private static CustomerAddressSaveDto CreateAddressDto(string street = "Rua A", long id = 0)
        {
            return new CustomerAddressSaveDto { Id = id, LocalityId = 1, Neighborhood = "Centro", Street = street, Number = "100" };
        }

        [Fact]
        public async Task CreateAsync_ValidDto_CreatesCustomerWithMappedAddresses()
        {
            IEnumerable<Entity> entities = await _service.CreateAsync([CreateDto(CreateAddressDto("Rua A"), CreateAddressDto("Rua B"))], TestContext.Current.CancellationToken);

            Customer customer = entities.ShouldHaveSingleItem().ShouldBeOfType<Customer>();
            customer.Name.ShouldBe("João da Silva");
            customer.DocumentType.ShouldBe(DocumentType.CPF);
            customer.Email.ShouldBe("joao@email.com");
            customer.Addresses.Select(a => a.Street).ShouldBe(["Rua A", "Rua B"], ignoreOrder: true);
            await _repository.Received(1).SaveAsync(Arg.Any<IEnumerable<Customer>>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdateAsync_ExistingCustomer_SynchronizesAddresses()
        {
            Customer existing = CustomerBuilder.Customer(addresses: [CustomerBuilder.Address(street: "Rua A"), CustomerBuilder.Address(street: "Rua B")]);
            existing.Id = 7;
            existing.Addresses.First(a => a.Street == "Rua A").Id = 1;
            existing.Addresses.First(a => a.Street == "Rua B").Id = 2;
            _repository.GetByIdsAsync(Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns([existing]);

            await _service.UpdateAsync(7, CreateDto(CreateAddressDto("Rua A alterada", id: 1), CreateAddressDto("Rua C")), TestContext.Current.CancellationToken);

            existing.Addresses.Count.ShouldBe(2);
            existing.Addresses.ShouldContain(a => a.Id == 1 && a.Street == "Rua A alterada");
            existing.Addresses.ShouldContain(a => a.Id == 0 && a.Street == "Rua C" && a.CustomerId == 7);
            await _repository.Received(1).SaveAsync(Arg.Any<IEnumerable<Customer>>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetSynchronizationAsync_DelegatesToQuery()
        {
            CustomerListingFilter filter = new();
            List<CustomerSynchronizationDto> synchronization = [];
            _query.GetSynchronization(filter, Arg.Any<CancellationToken>()).Returns(synchronization);

            List<CustomerSynchronizationDto> result = await _service.GetSynchronizationAsync(filter, TestContext.Current.CancellationToken);

            result.ShouldBeSameAs(synchronization);
        }
    }
}
