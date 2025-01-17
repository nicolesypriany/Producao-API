using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProducaoAPI.Models;

namespace ProducaoAPI.Data.Mappings
{
    public class ProcessoProducaoMateriaPrimaTypeConfiguration : IEntityTypeConfiguration<ProcessoProducaoMateriaPrima>
    {
        public void Configure(EntityTypeBuilder<ProcessoProducaoMateriaPrima> builder)
        {
            builder.ToTable("producoes_materias_primas");
            builder.Property(p => p.ProducaoId).HasColumnName("producao_id");
            builder.Property(p => p.MateriaPrimaId).HasColumnName("materia_prima_id");
            builder.Property(p => p.Quantidade).HasColumnName("quantidade");
            builder.Property(p => p.Ativo).HasColumnName("ativo");

            builder
                .HasKey(pp => new { pp.ProducaoId, pp.MateriaPrimaId });

            builder
                .HasOne(pp => pp.ProcessoProducao)
                .WithMany(p => p.ProducaoMateriasPrimas)
                .HasForeignKey(pp => pp.ProducaoId);

            builder
                .HasOne(pp => pp.MateriaPrima)
                .WithMany(p => p.ProducaoMateriasPrimas)
                .HasForeignKey(pp => pp.MateriaPrimaId);
        }
    }
}
