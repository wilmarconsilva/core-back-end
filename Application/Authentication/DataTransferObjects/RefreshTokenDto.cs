namespace Application.Authentication.DataTransferObjects
{
    public class RefreshTokenDto
    {
        public required string RefreshToken { get; init; }

        public required DateTime RefreshTokenExpirationDateTime { get; init; }
    }
}
