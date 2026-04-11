using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.Domain.Entities;

namespace Portal.Infra.Data.Repository.Data.Configurations
{
    public class VendedorConfiguration : IEntityTypeConfiguration<Vendedor>
    {
        public void Configure(EntityTypeBuilder<Vendedor> builder)
        {
            builder.ToTable("Vendedores");

            builder.HasKey(v => v.Id);
            builder.Property(v => v.Id)
                   .ValueGeneratedNever();

            builder.HasIndex(v => v.Cpf)
                   .IsUnique();

            builder.HasIndex(v => v.Email)
                   .IsUnique();

            builder.Property(v => v.NomeCompleto)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(v => v.PercentualComissao)
                   .HasPrecision(5, 2);

            builder.Property(v => v.CreatedAt)
                   .ValueGeneratedOnAdd();
        }
    }
}
