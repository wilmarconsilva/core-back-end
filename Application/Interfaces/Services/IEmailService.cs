using Application.DataTransferObjects.Email;

namespace Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailMessageDto message, CancellationToken cancellationToken);
    }
}
