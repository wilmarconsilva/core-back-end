namespace Application.Interfaces.Security;

/// <summary>
/// Usuário autenticado na requisição atual (derivado do principal HTTP / JWT).
/// </summary>
public interface IHttpContextCurrentUser
{
    /// <summary>Identificador do usuário autenticado. Lança <see cref="UnauthorizedAccessException"/> se não houver identidade válida ou claim de id.</summary>
    long UserId { get; }
}
