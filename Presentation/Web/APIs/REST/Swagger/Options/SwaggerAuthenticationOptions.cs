namespace REST.Swagger.Options;

/// <summary>
/// Credenciais de acesso (HTTP Basic) à documentação Swagger.
/// </summary>
public class SwaggerAuthenticationOptions
{
    public const string SectionName = "Swagger";

    public string Username { get; init; } = null!;

    public string? Password { get; init; }
}
