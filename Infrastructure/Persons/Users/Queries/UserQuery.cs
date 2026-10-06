using Application.Persons.Users.DataTransferObjects;
using Application.Persons.Users.Queries.Interfaces;
using Domain.Persons.Users.Entities;
using Infrastructure.Shared.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persons.Users.Queries
{
    public class UserQuery : AppDbContextAcessor, IUserQuery
    {
        public UserQuery(AppDbContext context) : base(context)
        {
        }

        public async Task<CurrentUserDto?> GetCurrentAsync(long userId, CancellationToken cancellationToken)
        {
            return await _context.Set<User>()
                .AsNoTracking()
                .Where(user => user.Id == userId)
                .Select(user => new CurrentUserDto
                {
                    Name = user.Name,
                    SignatureImage = user.SignatureImage,
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
