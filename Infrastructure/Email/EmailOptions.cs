namespace Infrastructure.Email
{
    public class EmailOptions
    {
        public const string SectionName = "Email";

        public string Host { get; private set; } = "localhost";

        public int Port { get; private set; } = 1025;

        public string SenderEmail { get; private set; } = "noreply@localhost";

        public string SenderName { get; private set; } = "Core Dev";

        public string? Username { get; private set; }

        public string? Password { get; private set; }

        public bool EnableSsl { get; private set; } = false;
    }
}
