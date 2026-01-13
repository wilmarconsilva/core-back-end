using FluentValidation;

namespace Application.DataTransferObjects.Persons.Customers.Validators
{
    public class CustomerSaveDtoValidator : Validator<CustomerSaveDto>
    {
        public CustomerSaveDtoValidator(IValidator<CustomerAddressSaveDto> customerAddressValidator)
        {
            RequiredString(c => c.Name);
            RequiredString(c => c.Document);

            RequiredEnum(c => c.PersonType);

            RuleForEach(c => c.Addresses).SetValidator(customerAddressValidator);
        }
    }
}