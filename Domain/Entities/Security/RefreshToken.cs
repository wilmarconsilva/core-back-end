using Domain.Entities.Persons;
using Domain.Utilities.Entities;

namespace Domain.Entities.Security
{
    public class RefreshToken : EntityWithoutDescription
    {
        private string _tokenHash = null!;
        private long _userId;

        private RefreshToken() { }

        public RefreshToken(long userId, string tokenHash, DateTime ExpirationDateTime)
        {
            CreatedDateTime = DateTime.UtcNow;
            SetUserId(userId);
            SetTokenHash(tokenHash);

            Validator.ValidateValue(ExpirationDateTime, "Data de expiração");
            ExpirationDateTime = ExpirationDateTime;
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
            Validator.ValidateValue(userId, "Código do usuário");
            _userId = userId;
        }

        private void SetTokenHash(string tokenHash)
        {
            Validator.ValidateValue(tokenHash, "Hash do Token");
            _tokenHash = tokenHash;
        }
    }
}
