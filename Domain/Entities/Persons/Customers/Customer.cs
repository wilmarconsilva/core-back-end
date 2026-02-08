using Domain.Exceptions;
using Domain.Utilities;
using Domain.ValueObjects.Persons.Enums;

namespace Domain.Entities.Persons.Customers
{
    public class Customer : Person
    {
        #region Fields

        private string _document = null!;
        private readonly List<CustomerAddress> _addresses = new();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        private Customer() { }

        public Customer(string name, PersonType personType, string document, string? email, string? stateRegistration, IEnumerable<CustomerAddress> addresses) : base(name)
        {
            SetPersonType(personType);
            SetAddresses(addresses);

            Document = document;
            Email = email;
            StateRegistration = stateRegistration;
        }

        #endregion Constructors

        #region Public properties

        public PersonType PersonType { get; private set; }

        public string Document { get => _document; private set => SetDocument(value); }

        public DocumentType DocumentType { get; private set; }

        public string? Email { get; set; }

        public string? StateRegistration { get; set; }

        public IReadOnlyCollection<CustomerAddress> Addresses => _addresses;

        #endregion Public properties

        #region Public methods

        public void SetPersonType(PersonType personType)
        {
            DocumentType = personType switch
            {
                PersonType.Natural => DocumentType.CPF,
                PersonType.Legal => DocumentType.CNPJ,
                _ => throw new BusinessRuleException($"Tipo de pessoa inválido para determinar o tipo de documento para o Cliente: {this.Description}.")
            };

            PersonType = personType;
        }

        public void SetDocument(string document)
        {
            Validate(document, nameof(Document));

            _document = document;
        }

        public void SetAddresses(IEnumerable<CustomerAddress> addresses)
        {
            if (addresses == null || !addresses.Any())
                throw new BusinessRuleException($"O Cliente: {this.Description} deve possuir ao menos um endereço.");

            EntityCollection.Sync(_addresses, addresses);
        }

        #endregion Public methods
    }
}