using System.Text;
using Domain.Entities.Territories;
using Domain.Interfaces.Entities;
using Domain.Utilities.Entities;

namespace Domain.Entities.Persons.Customers
{
    public class CustomerAddress : EntityWithoutDescription, ICopyable<CustomerAddress>, IEntityChild
    {
        #region Fields

        private long _localityId;
        private long _customerId;
        private string _neighborhood = null!;
        private string _street = null!;

        #endregion Fields

        #region Constructors

        public CustomerAddress(long localityId, long customerId, string neighborhood, string street, string? complement, string? postalCode, string? number)
        {
            SetCustomerId(customerId);

            Fill(localityId, neighborhood, street, complement, postalCode, number);
        }

        public CustomerAddress(long localityId, string neighborhood, string street, string? complement, string? postalCode, string? number)
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

        public string Neighborhood { get => _neighborhood; set => SetNeighborhood(value); }

        public string Street { get => _street; set => SetStreet(value); }

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

        #region Private methods

        private void Fill(long localityId, string neighborhood, string street, string? complement, string? postalCode, string? number)
        {
            SetLocalityId(localityId);
            SetNeighborhood(neighborhood);
            SetStreet(street);
            Complement = complement;
            PostalCode = postalCode;
            Number = number;
        }

        private void SetLocalityId(long localityId)
        {
            Validator.ValidateValue(localityId, nameof(LocalityId));

            _localityId = localityId;
        }

        private void SetCustomerId(long customerId)
        {
            Validator.ValidateValue(customerId, nameof(customerId));

            _customerId = customerId;
        }

        private void SetNeighborhood(string neighborhood)
        {
            Validator.ValidateValue(neighborhood, nameof(Neighborhood));

            _neighborhood = neighborhood;
        }

        private void SetStreet(string street)
        {
            Validator.ValidateValue(street, nameof(Street));

            _street = street;
        }

        #endregion Private methods
    }
}