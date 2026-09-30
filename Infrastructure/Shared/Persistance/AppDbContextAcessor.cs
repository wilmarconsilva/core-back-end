namespace Infrastructure.Shared.Persistance
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