using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProducaoAPI.Models;

namespace ProducaoAPI.Data.Mappings
{
    public class DespesaTypeConfiguration : IEntityTypeConfiguration<Despesa>
    {
        public void Configure(EntityTypeBuilder<Despesa> builder)
        {
            builder.ToTable("despesas");
            builder.Property(d => d.Id).HasColumnName("id");
            builder.Property(d => d.Nome).HasColumnName("nome");
            builder.Property(d => d.Descricao).HasColumnName("descricao");
            builder.Property(d => d.Valor).HasColumnName("valor");
            builder.Property(d => d.Ativo).HasColumnName("ativo");
        }
    }
}
