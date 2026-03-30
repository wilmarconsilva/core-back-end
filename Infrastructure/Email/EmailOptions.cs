namespace Infrastructure.Email
{
    public class EmailOptions
    {
        public const string SectionName = "Email";

        public string Host { get; private set; } = string.Empty;

        public int Port { get; private set; } = 587;

        public string SenderEmail { get; private set; } = string.Empty;

        public string SenderName { get; private set; } = string.Empty;

        public string Username { get; private set; } = string.Empty;

        public string Password { get; private set; } = string.Empty;

        public bool EnableSsl { get; private set; } = true;
    }
}
