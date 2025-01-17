using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProducaoAPI.Models;

namespace ProducaoAPI.Data.Mappings
{
    public class ProcessoProducaoTypeConfiguration : IEntityTypeConfiguration<ProcessoProducao>
    {
        public void Configure(EntityTypeBuilder<ProcessoProducao> builder)
        {
            builder.ToTable("producoes");
            builder.Property(p => p.Id).HasColumnName("id");
            builder.Property(p => p.Data).HasColumnName("data");
            builder.Property(p => p.MaquinaId).HasColumnName("maquina_id");
            builder.Property(p => p.FormaId).HasColumnName("forma_id");
            builder.Property(p => p.ProdutoId).HasColumnName("produto_id");
            builder.Property(p => p.Ciclos).HasColumnName("ciclos");
            builder.Property(p => p.QuantidadeProduzida).HasColumnName("quantidade_produzida");
            builder.Property(p => p.CustoUnitario).HasColumnName("custo_unitario");
            builder.Property(p => p.CustoTotal).HasColumnName("custo_total");
            builder.Property(p => p.Ativo).HasColumnName("ativo");

            builder
                .HasOne(p => p.Maquina)
                .WithMany(m => m.Producoes)
                .HasForeignKey(p => p.MaquinaId)
                .IsRequired();

            builder
                .HasOne(p => p.Forma)
                .WithMany(f => f.Producoes)
                .HasForeignKey(p => p.FormaId)
                .IsRequired();

            builder
                .HasOne(p => p.Produto)
                .WithMany(p => p.Producoes)
                .HasForeignKey(p => p.ProdutoId);
        }
    }
}
