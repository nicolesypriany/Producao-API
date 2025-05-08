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
        }
    }
}
