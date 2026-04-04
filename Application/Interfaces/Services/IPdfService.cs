namespace Application.Interfaces.Services
{
    public interface IPdfService
    {
        Task<byte[]> GenerateFromHtmlAsync(string html, CancellationToken cancellationToken);
    }
}
