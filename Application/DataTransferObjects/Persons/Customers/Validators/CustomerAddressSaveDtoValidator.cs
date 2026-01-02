using FluentValidation;

namespace Application.DataTransferObjects.Persons.Customers.Validators
{
    public class CustomerAddressSaveDtoValidator : Validator<CustomerAddressSaveDto>
    {
        public CustomerAddressSaveDtoValidator() 
        {
            RequiredString(c => c.Neighborhood).WithName("Bairro");
            RequiredString(c => c.Street).WithName("Logradouro");

            RequiredNumber(c => c.LocalityId).WithName("Localidade");
        }
    }
}