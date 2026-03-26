namespace Application.DataTransferObjects.Security.Authentications
{
    public class RefreshTokenDto
    {
        public required string RefreshToken { get; init; }

        public required DateTime RefreshTokenExpirationDateTime { get; init; }
    }
}
