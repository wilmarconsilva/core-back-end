namespace Infrastructure.Security.JsonWebTokens
{
    public class Options
    {
        public const string SectionName = "Jwt";

        public string Issuer { get; private set; } = string.Empty;

        public string Audience { get; private set; } = string.Empty;

        public string SigningKey { get; private set; } = string.Empty;

        public int AccessTokenExpirationMinutes { get; private set; } = 30;

        public int RefreshTokenExpirationDays { get; private set; } = 7;
    }
}
