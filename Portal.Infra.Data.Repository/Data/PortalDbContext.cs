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

        public DbSet<Vendedor> Vendedores { get; set; } = null!;
        public DbSet<Invoice> Invoices { get; set; } = null!;
        public DbSet<Comissao> Comissoes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PortalDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        // Dentro do seu PortalDbContext.cs
        public async Task<bool> ExisteCpf(string cpf) => await Vendedores.AnyAsync(v => v.Cpf == cpf);
        public async Task<bool> ExisteEmail(string email) => await Vendedores.AnyAsync(v => v.Email == email);
    }
}