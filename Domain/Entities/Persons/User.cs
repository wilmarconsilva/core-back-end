using Domain.Utilities.Entities;

namespace Domain.Entities.Persons
{
    public class User : Person
    {
        #region Fields

        private string _email = null!;
        private string _passwordHash = null!;

        #endregion Fields

        #region Constructors

        private User() { }

        public User(string name, string email, string passwordHash, byte[]? signatureImage = null) : base(name)
        {
            Fill(name, email, passwordHash, signatureImage);
        }

        #endregion Constructors

        public string Email { get => _email; set => SetEmail(value); }

        public string PasswordHash { get => _passwordHash; set => SetPasswordHash(value); }
        
        public byte[]? SignatureImage { get; set; }

        public void Fill(string name, string email, string passwordHash, byte[]? signatureImage = null)
        {
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            SignatureImage = signatureImage;
        }

        private void SetEmail(string email)
        {
            Validator.ValidateValue(email, nameof(Email));
            _email = email;
        }

        private void SetPasswordHash(string passwordHash)
        {
            Validator.ValidateValue(passwordHash, nameof(PasswordHash));
            _passwordHash = passwordHash;
        }
    }
}