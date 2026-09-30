using Application.Email.DataTransferObjects;

namespace Application.Email.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailMessageDto message, CancellationToken cancellationToken);
    }
}
