using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.Domain.Entities;

namespace Portal.Infra.Data.Repository.Data.Configurations
{
    public class ComissaoConfiguration : IEntityTypeConfiguration<Comissao>
    {
        public void Configure(EntityTypeBuilder<Comissao> builder)
        {
            builder.ToTable("Comissoes");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                   .ValueGeneratedNever(); 

            builder.Property(c => c.ValorBase)
                   .HasPrecision(18, 2);

            builder.Property(c => c.ValorComissao)
                   .HasPrecision(18, 2);

            builder.Property(c => c.PercentualAplicado)
                   .HasPrecision(5, 2);

            builder.HasOne(c => c.Invoice)
                   .WithOne(i => i.Comissao)
                   .HasForeignKey<Comissao>(c => c.InvoiceId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.CreatedAt)
                   .ValueGeneratedOnAdd();
        }
    }
}
