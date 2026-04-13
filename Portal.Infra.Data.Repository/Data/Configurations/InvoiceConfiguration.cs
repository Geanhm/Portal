using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.Domain.Entities;

namespace Portal.Infra.Data.Repository.Data.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                   .ValueGeneratedNever();

            builder.Property(i => i.Number)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(i => i.Number).IsUnique();

            builder.Property(i => i.Cliente)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(i => i.ClienteDocumento)
                   .IsRequired()
                   .HasMaxLength(14);

            builder.Property(i => i.ValorTotal)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(i => i.Observacoes)
                   .HasMaxLength(500);

            builder.Property(i => i.Status)
                   .IsRequired();

            builder.HasOne(i => i.Vendedor)
                   .WithMany()
                   .HasForeignKey(i => i.VendedorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(i => i.CreatedAt).ValueGeneratedOnAdd();
        }
    }
}
