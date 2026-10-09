using Domain.Persons.Users.Entities;
using Domain.Shared.Exceptions;

namespace Domain.UnitTests.Persons.Users.Entities
{
    public class UserTests
    {
        [Fact]
        public void Constructor_ValidData_FillsProperties()
        {
            byte[] signature = [1, 2, 3];

            User user = new("Maria", "maria@email.com", "hash", signature);

            user.Name.ShouldBe("Maria");
            user.Description.ShouldBe("Maria");
            user.Email.ShouldBe("maria@email.com");
            user.PasswordHash.ShouldBe("hash");
            user.SignatureImage.ShouldBe(signature);
        }

        [Theory]
        [InlineData("", "maria@email.com", "hash")]
        [InlineData("Maria", "", "hash")]
        [InlineData("Maria", "maria@email.com", "")]
        public void Constructor_MissingRequiredField_ThrowsBusinessRuleException(string name, string email, string passwordHash)
        {
            Should.Throw<BusinessRuleException>(() => new User(name, email, passwordHash));
        }
    }
}
