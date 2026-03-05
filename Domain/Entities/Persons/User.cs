using Domain.Utilities.Entities;

namespace Domain.Entities.Persons
{
    public class User : Person
    {
        private string _email = null!;
        private string _password = null!;

        public string Email { get => _email; private set => SetEmail(value); }

        public string Password { get => _password; private set => SetPassword(value); }
        
        public byte[]? SignatureImage { get; set; }

        private void SetEmail(string email)
        {
           Validator.ValidateValue(email, nameof(Email));
            Email = email;
        }

        private void SetPassword(string password)
        {
            Validator.ValidateValue(password, nameof(Password));
            Password = password;
        }
    }
}