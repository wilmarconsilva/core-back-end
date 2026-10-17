using System.Text;
using Domain.Shared.Entities;
using Domain.Shared.Interfaces;
using Domain.Shared.Utilities;
using Domain.Territories.Localities.Entities;

namespace Domain.Persons.Customers.Entities
{
    public class CustomerAddress : EntityWithoutDescription, ICopyable<CustomerAddress>, IEntityChild
    {
        #region Fields

        private long _localityId;
        private long _customerId;

        #endregion Fields

        #region Constructors

        public CustomerAddress(long localityId, long customerId, string? neighborhood, string? street, string? complement, string? postalCode, string? number)
        {
            SetCustomerId(customerId);

            Fill(localityId, neighborhood, street, complement, postalCode, number);
        }

        public CustomerAddress(long localityId, string? neighborhood, string? street, string? complement, string? postalCode, string? number)
        {
            Fill(localityId, neighborhood, street, complement, postalCode, number);
        }

        public void Copy(CustomerAddress customerAddress)
        {
            Fill(customerAddress.LocalityId, customerAddress.Neighborhood, customerAddress.Street, customerAddress.Complement, customerAddress.PostalCode, customerAddress.Number);
        }

        #endregion Constructors

        public Locality? Locality { get; private init; }

        public long LocalityId { get => _localityId; set => SetLocalityId(value); }

        public long CustomerId { get => _customerId; set => SetCustomerId(value); }

        public string? Neighborhood { get; set; }

        public string? Street { get; set; }

        public string? Complement { get; set; }

        public string? PostalCode { get; set; }

        public string? Number { get; set; }

        #region Public methods

        public void SetFatherEntityId(long id)
        {
            SetCustomerId(id);
        }

        #endregion Public methods

        #region Protected methods

        protected override string GetDescription()
        {
            StringBuilder description = new StringBuilder(string.Join(", ", new[] { Neighborhood, Street }.Where(part => !string.IsNullOrWhiteSpace(part))));

            if (!string.IsNullOrWhiteSpace(Number))
                description.Append(description.Length > 0 ? $" - {Number}" : Number);

            if (description.Length > 0)
                description.Append('.');

            if (!string.IsNullOrWhiteSpace(Complement))
                description.Append($" {Complement}.");

            if (!string.IsNullOrWhiteSpace(PostalCode))
                description.Append($" {PostalCode}.");

            return description.ToString().TrimStart();
        }

        #endregion Protected methods

        #region Private methods

        private void Fill(long localityId, string? neighborhood, string? street, string? complement, string? postalCode, string? number)
        {
            SetLocalityId(localityId);
            Neighborhood = neighborhood;
            Street = street;
            Complement = complement;
            PostalCode = postalCode;
            Number = number;
        }

        private void SetLocalityId(long localityId)
        {
            ValueValidatorUtility.ValidateValue(localityId, nameof(LocalityId));

            _localityId = localityId;
        }

        private void SetCustomerId(long customerId)
        {
            ValueValidatorUtility.ValidateValue(customerId, nameof(customerId));

            _customerId = customerId;
        }

        #endregion Private methods
    }
}
