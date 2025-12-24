using System.Text;
using Domain.Entities.Persons.Territories;

namespace Domain.Entities.Persons.Customers
{
    public class CustomerAddress : EntityWithoutDescription
    {
        public required string Neighborhood { get; set; }

        public required string Street { get; set; }

        public required Locality Locality { get; set; }

        public required Customer Customer { get; set; }

        public string? Complement { get; set; }

        public string? PostalCode { get; set; }
        
        public string? Number { get; set; }

        #region Protected methods

        protected override string GetDescription()
        {
            StringBuilder description = new StringBuilder($"{Neighborhood}, {Street}");

            if (!string.IsNullOrWhiteSpace(Number))
                description.Append($" - {Number}");

            description.Append('.');

            if (!string.IsNullOrWhiteSpace(Complement))
                description.Append($" {Complement}.");

            if (!string.IsNullOrWhiteSpace(PostalCode))
                description.Append($" {PostalCode}.");

            return description.Append('.').ToString();
        }

        #endregion Protected methods
    }
}