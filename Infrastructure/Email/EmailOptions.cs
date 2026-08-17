namespace Infrastructure.Email
{
    public class EmailOptions
    {
        public const string SectionName = "Email";

        public bool Enabled { get; init; }

        /// <summary>
        /// Required when Enabled is true.
        /// </summary>
        public string? Host { get; init; }

        /// <summary>
        /// Required when Enabled is true.
        /// </summary>
        public int? Port { get; init; }

        /// <summary>
        /// Required when Enabled is true.
        /// </summary>
        public string? SenderEmail { get; init; }

        public string? SenderName { get; init; }

        public string? Username { get; init; }

        public string? Password { get; init; }

        public bool EnableSsl { get; init; }
    }
}
