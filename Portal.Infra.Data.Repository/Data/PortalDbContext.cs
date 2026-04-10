using Microsoft.EntityFrameworkCore;
using Portal.Domain.Entities;

namespace Portal.Infra.Data.Repository
{
    public class PortalDbContext : DbContext
    {
        public PortalDbContext(DbContextOptions<PortalDbContext> options)
            : base(options)
        {
        }

        // Sample DbSet properties for your entities
        public DbSet<Vendedor> Vendedores { get; set; } = null!;
        public DbSet<Invoice> Invoices { get; set; } = null!;
        public DbSet<Comissao> Comissoes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações extras (tamanho de campos, índices) vão aqui
            //modelBuilder.Entity<Vendedor>().Property(u => u.Nome).HasMaxLength(100).IsRequired();
            base.OnModelCreating(modelBuilder);
        }
    }
}