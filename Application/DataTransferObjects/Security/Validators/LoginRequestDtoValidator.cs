using FluentValidation;

namespace Application.DataTransferObjects.Security.Validators
{
    public class LoginRequestDtoValidator : Validator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RequiredString(x => x.Email)
                .EmailAddress();

            RequiredString(x => x.Password);
        }
    }
}
