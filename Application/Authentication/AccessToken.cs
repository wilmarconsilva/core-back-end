namespace Application.Authentication
{
    public sealed class AccessToken
    {
        public required string Token { get; init; }

        public required DateTime ExpirationDateTime { get; init; }

        public required string UserName { get; init; }
    }
}
