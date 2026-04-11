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

            builder.HasIndex(i => i.Number)
                   .IsUnique();

            builder.Property(i => i.ValorTotal)
                   .HasPrecision(18, 2);

            builder.HasOne(i => i.Vendedor)
                   .WithMany()
                   .IsRequired()
                   .HasForeignKey(i => i.VendedorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(i => i.CreatedAt)
                   .ValueGeneratedOnAdd();
        }
    }
}
