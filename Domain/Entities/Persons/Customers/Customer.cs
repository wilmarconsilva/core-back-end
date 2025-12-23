using Domain.Exceptions;
using Domain.ValueObjects.Persons.Enums;

namespace Domain.Entities.Persons.Customers
{
    public class Customer : Person
    {
        #region Public properties

        public PersonType PersonType { get; private set; }

        public required string Document { get; set; }

        public DocumentType DocumentType { get; private set; }

        public string? Email { get; set; }

        public string? StateRegistration { get; set; }

        #region Constructors

        public Customer(PersonType personType, string document, string? email, string? stateRegistration)
        {
            PersonType = personType;
            Document = document;
            Email = email;
            StateRegistration = stateRegistration;

            SetPersonType(personType);
        }

        #endregion Constructors

        #endregion Public properties

        #region Private methods

        private void SetPersonType(PersonType personType)
        {
            DocumentType = personType switch
            {
                PersonType.Natural => DocumentType.CPF,
                PersonType.Legal => DocumentType.CNPJ,
                _ => throw new BusinessRuleException("Tipo de pessoa inválido para determinar o tipo de documento.")
            };

            PersonType = personType;
        }

        #endregion Private methods
    }
}