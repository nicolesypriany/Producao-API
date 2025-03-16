using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Models;

namespace ProducaoAPI.Data
{
    public class ProducaoContext : DbContext
    {
        public ProducaoContext(DbContextOptions<ProducaoContext> options) : base(options)
        {

        }

        public DbSet<Maquina> Maquinas { get; set; }
        public DbSet<Forma> Formas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<MateriaPrima> MateriasPrimas { get; set; }
        public DbSet<ProcessoProducao> Producoes { get; set; }
        public DbSet<ProcessoProducaoMateriaPrima> ProducoesMateriasPrimas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProducaoContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}