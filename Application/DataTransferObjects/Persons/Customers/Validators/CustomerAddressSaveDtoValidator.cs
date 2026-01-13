namespace Application.DataTransferObjects.Persons.Customers.Validators
{
    public class CustomerAddressSaveDtoValidator : Validator<CustomerAddressSaveDto>
    {
        public CustomerAddressSaveDtoValidator() 
        {
            RequiredString(c => c.Neighborhood);
            RequiredString(c => c.Street);

            RequiredNumber(c => c.LocalityId);
        }
    }
}