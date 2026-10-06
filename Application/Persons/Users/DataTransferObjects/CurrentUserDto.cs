namespace Application.Persons.Users.DataTransferObjects
{
    public class CurrentUserDto
    {
        public required string Name { get; init; }

        public byte[]? SignatureImage { get; init; }
    }
}
