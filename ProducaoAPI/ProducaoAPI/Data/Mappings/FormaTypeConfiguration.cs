using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProducaoAPI.Models;

namespace ProducaoAPI.Data.Mappings
{
    public class FormaTypeConfiguration : IEntityTypeConfiguration<Forma>
    {
        public void Configure(EntityTypeBuilder<Forma> builder)
        {
            builder.ToTable("formas");
            builder.Property(f => f.Id).HasColumnName("id");
            builder.Property(f => f.Nome).HasColumnName("nome");
            builder.Property(f => f.ProdutoId).HasColumnName("produto_id");
            builder.Property(f => f.PecasPorCiclo).HasColumnName("pecas_por_ciclo");
            builder.Property(f => f.Ativo).HasColumnName("ativo");

            builder
               .HasOne(f => f.Produto)
               .WithMany(p => p.Formas)
               .HasForeignKey(f => f.ProdutoId)
               .IsRequired();
        }
    }
}
