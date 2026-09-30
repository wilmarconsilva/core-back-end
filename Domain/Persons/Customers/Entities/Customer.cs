using Domain.Shared.Exceptions;
using Domain.Persons.Entities;
using Domain.Persons.Enums;
using Domain.Shared.Utilities;

namespace Domain.Persons.Customers.Entities
{
    public class Customer : Person
    {
        #region Fields

        private string _document = null!;
        private string? _email = null;
        private readonly List<CustomerAddress> _addresses = new();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        private Customer() { }

        public Customer(string name, PersonType personType, string document, string? email, string? stateRegistration, IReadOnlyCollection<CustomerAddress> addresses) : base(name)
        {
            Fill(name, personType, document, email, stateRegistration, addresses);
        }

        #endregion Constructors

        #region Public properties

        public PersonType PersonType { get; private set; }

        public string Document { get => _document; set => SetDocument(value); }

        public DocumentType DocumentType { get; private set; }

        public string? Email { get => _email; set => SetEmail(value); }

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
                _ => throw new BusinessRuleException($"Tipo de pessoa inválido para determinar o tipo de documento para o cliente: {this.Description}.")
            };

            PersonType = personType;
        }

        public void SetAddresses(IReadOnlyCollection<CustomerAddress> addresses)
        {
            if (addresses == null || !addresses.Any())
                throw new BusinessRuleException($"O cliente: {this.Description} deve possuir ao menos um endereço.");

            EntityCollectionUtility.Sync(_addresses, addresses, this.Id);
        }

        public void Fill(string name, PersonType personType, string document, string? email, string? stateRegistration, IReadOnlyCollection<CustomerAddress> addresses)
        {
            Name = name;
            Email = email;
            StateRegistration = stateRegistration;

            SetPersonType(personType);
            Document = document;

            SetAddresses(addresses);
        }

        private void SetDocument(string document)
        {
            ValueValidatorUtility.ValidateCpfCnpj(document, DocumentType.ToString(), this.PersonType);

            _document = document;
        }

        private void SetEmail(string? email)
        {
            if (email is null)
                return;

            ValueValidatorUtility.ValidateEmail(email, "E-mail");

            _email = email;
        }

        #endregion Public methods
    }
}
 