using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProducaoAPI.Models;

namespace ProducaoAPI.Data.Mappings
{
    public class LogTypeConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("logs");
            builder.Property(l => l.Id).HasColumnName("id");
            builder.Property(l => l.Acao).HasColumnName("acao");
            builder.Property(l => l.Objeto).HasColumnName("objeto");
            builder.Property(l => l.IdObjeto).HasColumnName("id_objeto");
            builder.Property(l => l.Data).HasColumnName("data");
            builder.Property(l => l.DadoAlterado).HasColumnName("dado_alterado");
            builder.Property(l => l.Conteudo).HasColumnName("conteudo");
            builder.Property(l => l.UserId).HasColumnName("user_id");

        }
    }
}
