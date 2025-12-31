using FluentValidation;

namespace Application.DataTransferObjects.Persons.Customers.Validators
{
    public class CustomerSaveDtoValidator : Validator<CustomerSaveDto>
    {
        public CustomerSaveDtoValidator() 
        {
            RequiredString(c => c.Name);
            RequiredString(c => c.Document);
        }
    }
}