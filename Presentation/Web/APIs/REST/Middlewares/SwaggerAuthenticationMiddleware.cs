using Microsoft.Extensions.Options;
using REST.Options;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace REST.Middlewares;

/// <summary>
/// Protege a documentação Swagger com HTTP Basic, validando as credenciais de <see cref="SwaggerAuthenticationOptions"/>.
/// </summary>
public class SwaggerAuthenticationMiddleware
{
    private const string Scheme = "Basic";

    private readonly RequestDelegate _next;
    private readonly byte[] _username;
    private readonly byte[] _password;

    public SwaggerAuthenticationMiddleware(RequestDelegate next, IOptions<SwaggerAuthenticationOptions> options)
    {
        _next = next;
        _username = Encoding.UTF8.GetBytes(options.Value.Username);
        _password = Encoding.UTF8.GetBytes(options.Value.Password!);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (IsAuthorized(context.Request))
        {
            await _next(context);
            return;
        }

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.Headers.WWWAuthenticate = $"{Scheme} realm=\"Swagger\", charset=\"UTF-8\"";
    }

    private bool IsAuthorized(HttpRequest request)
    {
        if (!AuthenticationHeaderValue.TryParse(request.Headers.Authorization, out AuthenticationHeaderValue? header)
            || !Scheme.Equals(header.Scheme, StringComparison.OrdinalIgnoreCase)
            || string.IsNullOrEmpty(header.Parameter))
            return false;

        string credentials;
        try
        {
            credentials = Encoding.UTF8.GetString(Convert.FromBase64String(header.Parameter));
        }
        catch (FormatException)
        {
            return false;
        }

        int separatorIndex = credentials.IndexOf(':');
        if (separatorIndex < 0)
            return false;

        byte[] username = Encoding.UTF8.GetBytes(credentials[..separatorIndex]);
        byte[] password = Encoding.UTF8.GetBytes(credentials[(separatorIndex + 1)..]);

        // Operador "&" (sem curto-circuito) para que as duas comparações sempre sejam executadas.
        return CryptographicOperations.FixedTimeEquals(username, _username)
            & CryptographicOperations.FixedTimeEquals(password, _password);
    }
}
