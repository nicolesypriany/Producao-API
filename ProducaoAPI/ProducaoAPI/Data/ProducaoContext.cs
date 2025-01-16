using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Models;

namespace ProducaoAPI.Data
{
    public class ProducaoContext : IdentityDbContext<PessoaComAcesso, PerfilDeAcesso, int>
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

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            var maquina = modelBuilder.Entity<Maquina>();

            maquina.ToTable("maquinas");
            maquina.Property(m => m.Id).HasColumnName("id");
            maquina.Property(m => m.Nome).HasColumnName("nome");
            maquina.Property(m => m.Marca).HasColumnName("marca");
            maquina.Property(m => m.Ativo).HasColumnName("ativo");

            var formas = modelBuilder.Entity<Forma>();

            formas.ToTable("formas");
            formas.Property(f => f.Id).HasColumnName("id");
            formas.Property(f => f.Nome).HasColumnName("nome");
            formas.Property(f => f.ProdutoId).HasColumnName("produto_id");
            formas.Property(f => f.PecasPorCiclo).HasColumnName("pecas_por_ciclo");
            formas.Property(f => f.Ativo).HasColumnName("ativo");

            var produtos = modelBuilder.Entity<Produto>();

            produtos.ToTable("produtos");
            produtos.Property(p => p.Id).HasColumnName("id");
            produtos.Property(p => p.Nome).HasColumnName("nome");
            produtos.Property(p => p.Medidas).HasColumnName("medidas");
            produtos.Property(p => p.Unidade).HasColumnName("unidade");
            produtos.Property(p => p.PecasPorUnidade).HasColumnName("pecas_por_unidade");
            produtos.Property(p => p.Ativo).HasColumnName("ativo");

            var materiasPrimas = modelBuilder.Entity<MateriaPrima>();

            materiasPrimas.ToTable("materias_primas");
            materiasPrimas.Property(m => m.Id).HasColumnName("id");
            materiasPrimas.Property(m => m.Nome).HasColumnName("nome");
            materiasPrimas.Property(m => m.Fornecedor).HasColumnName("fornecedor");
            materiasPrimas.Property(m => m.Unidade).HasColumnName("unidade");
            materiasPrimas.Property(m => m.Preco).HasColumnName("preco");
            materiasPrimas.Property(m => m.Ativo).HasColumnName("ativo");

            var producoes = modelBuilder.Entity<ProcessoProducao>();

            producoes.ToTable("producoes");
            producoes.Property(p => p.Id).HasColumnName("id");
            producoes.Property(p => p.Data).HasColumnName("data");
            producoes.Property(p => p.MaquinaId).HasColumnName("maquina_id");
            producoes.Property(p => p.FormaId).HasColumnName("forma_id");
            producoes.Property(p => p.ProdutoId).HasColumnName("produto_id");
            producoes.Property(p => p.Ciclos).HasColumnName("ciclos");
            producoes.Property(p => p.QuantidadeProduzida).HasColumnName("quantidade_produzida");
            producoes.Property(p => p.CustoUnitario).HasColumnName("custo_unitario");
            producoes.Property(p => p.CustoTotal).HasColumnName("custo_total");
            producoes.Property(p => p.Ativo).HasColumnName("ativo");

            var producoeMaterias = modelBuilder.Entity<ProcessoProducaoMateriaPrima>();

            producoeMaterias.ToTable("producoes_materias_primas");
            producoeMaterias.Property(p => p.ProducaoId).HasColumnName("producao_id");
            producoeMaterias.Property(p => p.MateriaPrimaId).HasColumnName("materia_prima_id");
            producoeMaterias.Property(p => p.Quantidade).HasColumnName("quantidade");
            producoeMaterias.Property(p => p.Ativo).HasColumnName("ativo");

            modelBuilder.Entity<Maquina>()
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


            modelBuilder.Entity<Forma>()
                .HasOne(f => f.Produto)
                .WithMany(p => p.Formas)
                .HasForeignKey(f => f.ProdutoId)
                .IsRequired();

            modelBuilder.Entity<ProcessoProducao>()
                .HasOne(p => p.Maquina)
                .WithMany(m => m.Producoes)
                .HasForeignKey(p => p.MaquinaId)
                .IsRequired();

            modelBuilder.Entity<ProcessoProducao>()
                            .HasOne(p => p.Forma)
                            .WithMany(f => f.Producoes)
                            .HasForeignKey(p => p.FormaId)
                            .IsRequired();

            modelBuilder.Entity<ProcessoProducao>()
                .HasOne(p => p.Produto)
                .WithMany(p => p.Producoes)
                .HasForeignKey(p => p.ProdutoId);

            modelBuilder.Entity<ProcessoProducaoMateriaPrima>()
                .HasKey(pp => new { pp.ProducaoId, pp.MateriaPrimaId });

            modelBuilder.Entity<ProcessoProducaoMateriaPrima>()
                .HasOne(pp => pp.ProcessoProducao)
                .WithMany(p => p.ProducaoMateriasPrimas)
                .HasForeignKey(pp => pp.ProducaoId);

            modelBuilder.Entity<ProcessoProducaoMateriaPrima>()
                .HasOne(pp => pp.MateriaPrima)
                .WithMany(p => p.ProducaoMateriasPrimas)
                .HasForeignKey(pp => pp.MateriaPrimaId);

        }
    }
}