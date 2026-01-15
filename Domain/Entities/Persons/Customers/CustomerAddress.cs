using System.Text;
using Domain.Entities.Territories;
using Domain.Interfaces.Entities;

namespace Domain.Entities.Persons.Customers
{
    public class CustomerAddress : EntityWithoutDescription, ISyncable<CustomerAddress>
    {
        public Locality? Locality { get; private set; }

        public required long LocalityId { get; set; }

        public Customer? Customer { get; private set; }

        public required long CustomerId { get; set; }

        public required string Neighborhood { get; set; }

        public required string Street { get; set; }

        public string? Complement { get; set; }

        public string? PostalCode { get; set; }
        
        public string? Number { get; set; }

        #region Public methods

        public void Sync(CustomerAddress customerAddress)
        {
            LocalityId = customerAddress.LocalityId;
            CustomerId = customerAddress.CustomerId;
            Neighborhood = customerAddress.Neighborhood;
            Street = customerAddress.Street;
            Complement = customerAddress.Complement;
            PostalCode = customerAddress.PostalCode;
            Number = customerAddress.Number;
        }

        #endregion Public methods

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

            return description.ToString();
        }

        #endregion Protected methods
    }
}