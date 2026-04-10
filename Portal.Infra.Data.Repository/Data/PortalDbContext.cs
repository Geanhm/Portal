using Microsoft.EntityFrameworkCore;

namespace Portal.Infra.Data.Repository
{
    public class PortalDbContext : DbContext
    {
        public PortalDbContext(DbContextOptions<PortalDbContext> options)
            : base(options)
        {
        }

        // Add your DbSet<T> properties here, for example:
        // public DbSet<User> Users { get; set; }
    }
}