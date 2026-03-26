namespace Application.DataTransferObjects.Security.Authentications
{
    public class AccessTokenDto
    {
        public required string AccessToken { get; init; }

        public required DateTime AccessTokenExpirationDateTime { get; init; }
    }
}
