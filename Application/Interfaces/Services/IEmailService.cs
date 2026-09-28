using Application.Email.DataTransferObjects;

namespace Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailMessageDto message, CancellationToken cancellationToken);
    }
}
