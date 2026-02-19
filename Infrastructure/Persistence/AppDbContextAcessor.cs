using Infrastructure.Persistece;

namespace Infrastructure.Persistence
{
    public abstract class AppDbContextAcessor
    {
        protected readonly AppDbContext _context;

        protected AppDbContextAcessor(AppDbContext context)
        {
            _context = context;
        }
    }
}