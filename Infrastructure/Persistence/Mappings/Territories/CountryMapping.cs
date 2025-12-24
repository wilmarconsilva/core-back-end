using Domain.Entities.Territories;

namespace Infrastructure.Persistence.Mappings.Territories
{
    public class CountryMapping : TerritoryMapping<Country>
    {
        protected override string GetTableName()
        {
            return "countries";
        }
    }
}