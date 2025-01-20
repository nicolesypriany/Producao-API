using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProducaoAPI.Models;

namespace ProducaoAPI.Data.Mappings
{
    public class ProdutoTypeConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("produtos");
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Nome).HasColumnName("nome");
            builder.Property(p => p.Medidas).HasColumnName("medidas");
            builder.Property(p => p.Unidade).HasColumnName("unidade");
            builder.Property(p => p.PecasPorUnidade).HasColumnName("pecas_por_unidade");
            builder.Property(p => p.Ativo).HasColumnName("ativo");
        }
    }
}
