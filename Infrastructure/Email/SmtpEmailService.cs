using System.Net;
using System.Net.Mail;
using Application.Email.DataTransferObjects;
using Application.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace Infrastructure.Email
{
    public class SmtpEmailService : IEmailService
    {
        private readonly EmailOptions _options;

        public SmtpEmailService(IOptions<EmailOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendAsync(EmailMessageDto message, CancellationToken cancellationToken)
        {
            if (!_options.Enabled)
                throw new InvalidOperationException(
                    $"Email service is disabled. Set the environment variable: {EmailOptions.SectionName}__{nameof(EmailOptions.Enabled)} to true to enable it.");

            using SmtpClient client = new SmtpClient(_options.Host, _options.Port!.Value)
            {
                EnableSsl = _options.EnableSsl,
                Credentials = new NetworkCredential(_options.Username, _options.Password)
            };

            using MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(_options.SenderEmail!, _options.SenderName),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(message.To);

            foreach (var attachment in message.Attachments)
                mailMessage.Attachments.Add(new Attachment(new MemoryStream(attachment.Content), attachment.FileName, attachment.MediaType));

            await client.SendMailAsync(mailMessage, cancellationToken);
        }
    }
}
