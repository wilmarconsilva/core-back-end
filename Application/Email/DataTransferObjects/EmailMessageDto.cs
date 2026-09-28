namespace Application.Email.DataTransferObjects
{
    public class EmailMessageDto
    {
        public EmailMessageDto(string to, string subject, string body, IReadOnlyCollection<EmailAttachmentDto>? attachments = null)
        {
            To = to;
            Subject = subject;
            Body = body;
            Attachments = attachments ?? [];
        }

        public string To { get; private init; }

        public string Subject { get; private init; }

        public string Body { get; private init; }

        public IReadOnlyCollection<EmailAttachmentDto> Attachments { get; private init; }
    }
}
