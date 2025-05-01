using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Models;

namespace ProducaoAPI.Data.Mappings
{
    public class UserTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("usuarios");
            builder.Property(u => u.Id).HasColumnName("id");
            builder.Property(u => u.Nome).HasColumnName("nome");
            builder.Property(u => u.Email).HasColumnName("email");
            builder.Property(u => u.Cargo).HasColumnName("cargo");
            builder.Property(u => u.PasswordHash).HasColumnName("password_hash");
            builder.Property(u => u.PasswordSalt).HasColumnName("password_salt");
        }
    }
}