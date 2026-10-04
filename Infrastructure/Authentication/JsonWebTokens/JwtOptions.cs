namespace Infrastructure.Authentication.JsonWebTokens
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Issuer { get; init; } = "Core.Api";

        public string Audience { get; init; } = "Core.Client";

        public string? SigningKey { get; init; }

        public int AccessTokenExpirationMinutes { get; init; } = 30;

        public int RefreshTokenExpirationDays { get; init; } = 7;
    }
}
