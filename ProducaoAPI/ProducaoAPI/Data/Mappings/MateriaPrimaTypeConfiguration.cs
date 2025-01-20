using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProducaoAPI.Models;

namespace ProducaoAPI.Data.Mappings
{
    public class MateriaPrimaTypeConfiguration : IEntityTypeConfiguration<MateriaPrima>
    {
        public void Configure(EntityTypeBuilder<MateriaPrima> builder)
        {
            builder.ToTable("materias_primas");
            builder.Property(m => m.Id).HasColumnName("id");
            builder.Property(m => m.Nome).HasColumnName("nome");
            builder.Property(m => m.Fornecedor).HasColumnName("fornecedor");
            builder.Property(m => m.Unidade).HasColumnName("unidade");
            builder.Property(m => m.Preco).HasColumnName("preco");
            builder.Property(m => m.Ativo).HasColumnName("ativo");
        }
    }
}
