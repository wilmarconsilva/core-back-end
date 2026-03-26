namespace Application.DataTransferObjects.Security.Validators
{
    public class RefreshTokenRequestDtoValidator : Validator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestDtoValidator()
        {
            RequiredString(x => x.RefreshToken);
        }
    }
}