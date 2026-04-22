namespace Application.Models.Security
{
    public sealed class AccessTokenResult
    {
        public required string Token { get; init; }

        public required DateTime ExpirationDateTime { get; init; }

        public required string UserName { get; init; }
    }
}
