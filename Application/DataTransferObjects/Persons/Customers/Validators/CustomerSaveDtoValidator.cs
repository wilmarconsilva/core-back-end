using FluentValidation;

namespace Application.DataTransferObjects.Persons.Customers.Validators
{
    public class CustomerSaveDtoValidator : Validator<CustomerSaveDto>
    {
        public CustomerSaveDtoValidator(IValidator<CustomerAddressSaveDto> customerAddressValidator)
        {
            RequiredString(c => c.Name).WithName("Nome");
            RequiredString(c => c.Document).WithName("CPF/CNPJ");

            RequiredEnum(c => c.PersonType).WithName("Tipo de Pessoa");


            RuleFor(x => x.Addresses)
                .Must(x => x?.Count > 0)
                .WithMessage(_requiredEnumerableMessage)
                .WithName(x => $"Endereço para o Cliente: {x.Name}");

            RuleForEach(c => c.Addresses).SetValidator(customerAddressValidator);
        }
    }
}