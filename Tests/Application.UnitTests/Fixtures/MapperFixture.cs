using Application.Persons.Customers.Mappings;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace Application.UnitTests.Fixtures
{
    /// <summary>
    /// AutoMapper real, com os mesmos Profiles registrados na aplicação (varredura do assembly de Application).
    /// </summary>
    public sealed class MapperFixture
    {
        public MapperFixture()
        {
            Configuration = new MapperConfiguration(cfg => cfg.AddMaps(typeof(CustomerProfile).Assembly), NullLoggerFactory.Instance);
            Mapper = Configuration.CreateMapper();
        }

        public MapperConfiguration Configuration { get; }

        public IMapper Mapper { get; }
    }
}
