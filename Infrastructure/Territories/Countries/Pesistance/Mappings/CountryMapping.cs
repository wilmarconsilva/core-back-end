using Domain.Territories.Entities;
using Infrastructure.Territories.Persistance.Mappings;

namespace Infrastructure.Territories.Countries.Pesistance.Mappings
{
    public class CountryMapping : TerritoryMapping<Country>
    {
        protected override string GetTableName()
        {
            return "countries";
        }
    }
}