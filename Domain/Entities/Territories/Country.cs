using System.Xml.Linq;

namespace Domain.Entities.Territories
{
    public class Country : Territory
    {
        public Country(string acronym) : base(acronym) { }

        public override required string Description { get; set; }
    }
}