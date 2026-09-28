using Domain.Persons.Users.Entities;
using Domain.Shared.Entities;
using Domain.Shared.Utilities;

namespace Domain.Authtentication.Entities
{
    public class RefreshToken : EntityWithoutDescription
    {
        private string _tokenHash = null!;
        private long _userId;

        private RefreshToken() { }

        public RefreshToken(long userId, string tokenHash, DateTime expirationDateTime)
        {
            CreatedDateTime = DateTime.UtcNow;

            SetUserId(userId);
            SetTokenHash(tokenHash);

            ValueValidatorUtility.ValidateValue(expirationDateTime, "Data de expiração");
            ExpirationDateTime = expirationDateTime;
        }

        public long UserId { get => _userId; set => SetUserId(value); }

        public User? User { get; private set; }

        public string TokenHash { get => _tokenHash; set => SetTokenHash(value); }

        public DateTime CreatedDateTime { get; private init; }

        public DateTime ExpirationDateTime { get; private init; }

        public DateTime? RevokedDateTime { get; private set; }

        public string? ReplacedByTokenHash { get; private set; }

        public bool IsExpired => DateTime.UtcNow >= ExpirationDateTime;

        public bool IsRevoked => RevokedDateTime.HasValue;

        public void Rotate(string replacedByTokenHash)
        {
            Revoke();
            ReplacedByTokenHash = replacedByTokenHash;
        }

        public void Revoke()
        {
            RevokedDateTime ??= DateTime.UtcNow;
        }

        protected override string GetDescription()
        {
            return Id.ToString();
        }

        private void SetUserId(long userId)
        {
            ValueValidatorUtility.ValidateValue(userId, "Código do usuário");
            _userId = userId;
        }

        private void SetTokenHash(string tokenHash)
        {
            ValueValidatorUtility.ValidateValue(tokenHash, "Hash do Token");
            _tokenHash = tokenHash;
        }
    }
}
