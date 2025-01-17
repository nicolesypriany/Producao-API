using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProducaoAPI.Models;

namespace ProducaoAPI.Data.Mappings
{
    public class MaquinaTypeConfiguration : IEntityTypeConfiguration<Maquina>
    {
        public void Configure(EntityTypeBuilder<Maquina> builder)
        {
            builder.ToTable("maquinas");
            builder.Property(m => m.Id).HasColumnName("id");
            builder.Property(m => m.Nome).HasColumnName("nome");
            builder.Property(m => m.Marca).HasColumnName("marca");
            builder.Property(m => m.Ativo).HasColumnName("ativo");

            builder
                .HasMany(a => a.Formas)
                .WithMany(f => f.Maquinas)
                .UsingEntity<Dictionary<string, object>>(
                    "forma_maquina", // Nome da tabela de junção no banco de dados
                    right => right.HasOne<Forma>() // Configuração para a relação com a entidade Forma
                                    .WithMany()
                                    .HasForeignKey("FormaId") // Nome da coluna FK para Forma
                                    .HasConstraintName("FK_MaquinaForma_Forma"),
                    left => left.HasOne<Maquina>() // Configuração para a relação com a entidade Maquina
                                    .WithMany()
                                    .HasForeignKey("MaquinaId") // Nome da coluna FK para Maquina
                                    .HasConstraintName("FK_MaquinaForma_Maquina"),
                    join => // Configurações adicionais para a tabela de junção
                    {
                        join.ToTable("forma_maquina"); // Nome da tabela de junção
                        join.Property<int>("MaquinaId").HasColumnName("maquinas_id"); // Nome da coluna para MaquinaId
                        join.Property<int>("FormaId").HasColumnName("formas_id"); // Nome da coluna para FormaId
                    });
        }
    }
}
