using Domain.Authentication.Entities;
using Domain.Shared.Exceptions;

namespace Domain.UnitTests.Authentication.Entities
{
    public class RefreshTokenTests
    {
        private static RefreshToken CreateToken(DateTime? expirationDateTime = null)
        {
            return new RefreshToken(1, "hash", expirationDateTime ?? DateTime.UtcNow.AddDays(1));
        }

        [Fact]
        public void Constructor_ValidData_StartsActive()
        {
            RefreshToken token = CreateToken();

            token.UserId.ShouldBe(1);
            token.TokenHash.ShouldBe("hash");
            token.IsExpired.ShouldBeFalse();
            token.IsRevoked.ShouldBeFalse();
            token.CreatedDateTime.ShouldBe(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        }

        [Fact]
        public void Constructor_ZeroUserId_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new RefreshToken(0, "hash", DateTime.UtcNow.AddDays(1)));
        }

        [Fact]
        public void Constructor_EmptyHash_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new RefreshToken(1, "", DateTime.UtcNow.AddDays(1)));
        }

        [Fact]
        public void Constructor_MinExpirationDateTime_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new RefreshToken(1, "hash", DateTime.MinValue));
        }

        [Fact]
        public void IsExpired_PastExpiration_ReturnsTrue()
        {
            RefreshToken token = CreateToken(DateTime.UtcNow.AddSeconds(-1));

            token.IsExpired.ShouldBeTrue();
        }

        [Fact]
        public void Revoke_CalledTwice_KeepsFirstRevocationDate()
        {
            RefreshToken token = CreateToken();
            token.Revoke();
            DateTime? firstRevocation = token.RevokedDateTime;

            token.Revoke();

            token.IsRevoked.ShouldBeTrue();
            token.RevokedDateTime.ShouldBe(firstRevocation);
        }

        [Fact]
        public void Rotate_ActiveToken_RevokesAndStoresReplacement()
        {
            RefreshToken token = CreateToken();

            token.Rotate("novo-hash");

            token.IsRevoked.ShouldBeTrue();
            token.ReplacedByTokenHash.ShouldBe("novo-hash");
        }
    }
}
