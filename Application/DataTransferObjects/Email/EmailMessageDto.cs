namespace Application.DataTransferObjects.Email
{
    public class EmailMessageDto
    {
        public EmailMessageDto(string to, string subject, string body)
        {
            To = to;
            Subject = subject;
            Body = body;
        }

        public string To { get; private init; }

        public string Subject { get; private init; }

        public string Body { get; private init; }
    }
}
