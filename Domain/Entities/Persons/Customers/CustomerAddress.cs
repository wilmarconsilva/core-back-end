using System.Text;
using Domain.Entities.Territories;
using Domain.Interfaces.Entities;

namespace Domain.Entities.Persons.Customers
{
    public class CustomerAddress : EntityWithoutDescription, ICopyable<CustomerAddress>
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
            LocalityId = localityId;
            CustomerId = customerId;
            Neighborhood = neighborhood;
            Street = street;
            Complement = complement;
            PostalCode = postalCode;
            Number = number;
        }

        public void Copy(CustomerAddress customerAddress)
        {
            Locality = customerAddress.Locality;
            LocalityId = customerAddress.LocalityId;
            Customer = customerAddress.Customer;
            CustomerId = customerAddress.CustomerId;
            Neighborhood = customerAddress.Neighborhood;
            Street = customerAddress.Street;
            Complement = customerAddress.Complement;
            PostalCode = customerAddress.PostalCode;
            Number = customerAddress.Number;
        }

        #endregion Constructors

        public Locality? Locality { get; private set; }

        public long LocalityId { get => _localityId; private set => SetLocalityId(value); }

        public Customer? Customer { get; private set; }

        public long CustomerId { get => _customerId; private set => SetCustomerId(value); }

        public string Neighborhood { get => _neighborhood; private set => SetNeighborhood(value); }

        public string Street { get => _street; private set => SetNeighborhood(value); }

        public string? Complement { get; set; }

        public string? PostalCode { get; set; }
        
        public string? Number { get; set; }

        #region Public methods

        public void SetLocalityId(long localityId)
        {
            Validate(localityId, nameof(LocalityId));

            _localityId = localityId;
        }

        public void SetCustomerId(long customerId)
        {
            Validate(customerId, nameof(customerId));

            _customerId = customerId;
        }

        public void SetNeighborhood(string neighborhood)
        {
            Validate(Neighborhood, nameof(Neighborhood));

            _neighborhood = neighborhood;
        }

        public void SetStreet(string street)
        {
            Validate(street, nameof(Street));

            _street = street;
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