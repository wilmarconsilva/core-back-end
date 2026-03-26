namespace Infrastructure.Security.JsonWebTokens
{
    public class Options
    {
        public const string SectionName = "Jwt";

        public Options()
        {

        }

        public Options(string issuer, string audience, string signingKey, int? accessTokenExpirationMinutes, int? refreshTokenExpirationDays)
        {
            Issuer = issuer;
            Audience = audience;
            SigningKey = signingKey;
            AccessTokenExpirationMinutes = accessTokenExpirationMinutes ?? 30;
            RefreshTokenExpirationDays = refreshTokenExpirationDays ?? 7;
        }

        public string Issuer { get; private set; } = "Core.Api";

        public string Audience { get; private set; } = "Core.Client";

        public string SigningKey { get; private set; } = "CHANGE-ME-WITH-A-STRONG-KEY-AT-LEAST-32-CHARS";

        public int AccessTokenExpirationMinutes { get; private set; } = 30;

        public int RefreshTokenExpirationDays { get; private set; } = 7;
    }
}
