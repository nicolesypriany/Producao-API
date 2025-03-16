using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Services;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Test.ProducaoTests.Services
{
    public class Producao_Services
    {
        public ProducaoContext Context { get; }
        public IMateriaPrimaRepository MateriaPrimaRepository { get; }
        public IFormaRepository FormaRepository { get; }
        public IProdutoRepository ProdutoRepository { get; }
        public IProducaoMateriaPrimaRepository ProducaoMateriaPrimaRepository { get; }
        public IProducaoMateriaPrimaService ProducaoMateriaPrimaService { get; }
        public IMaquinaRepository MaquinaRepository { get; }
        public IProcessoProducaoRepository ProcessoProducaoRepository { get; }
        public IProcessoProducaoService ProcessoProducaoService { get; }

        public Producao_Services()
        {
            var options = new DbContextOptionsBuilder<ProducaoContext>()
                           .UseInMemoryDatabase("Teste")
                           .Options;

            Context = new ProducaoContext(options);
            ProcessoProducaoRepository = new ProcessoProducaoRepository(Context);
            MaquinaRepository = new MaquinaRepository(Context);
            ProducaoMateriaPrimaRepository = new ProducaoMateriaPrimaRepository(Context);
            ProdutoRepository = new ProdutoRepository(Context);
            FormaRepository = new FormaRepository(Context);
            MateriaPrimaRepository = new MateriaPrimaRepository(Context);
            ProducaoMateriaPrimaService = new ProducaoMateriaPrimaServices(ProducaoMateriaPrimaRepository, Context);
            ProcessoProducaoService = new ProcessoProducaoServices(ProcessoProducaoRepository, MateriaPrimaRepository, FormaRepository, ProdutoRepository, ProducaoMateriaPrimaRepository, ProducaoMateriaPrimaService, MaquinaRepository);
        }

        [Theory]
        [InlineData(0, 100, "O número de 'Ciclos' deve ser maior do que 0.")]
        [InlineData(100, 0, "\"O valor de 'Quantidade de Matéria-Prima' deve ser maior do que 0.\"")]
        public async Task RetornaErroAoValidarDados(int ciclos, int quantidadeMateriaPrima, string errorMessage)
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();
            await Context.Maquinas.AddAsync(new Maquina("teste", "teste"));
            await Context.Produtos.AddAsync(new Produto("teste", "teste", "un", 10));
            await Context.Formas.AddAsync(new Forma("teste", 1, 10));
            await Context.MateriasPrimas.AddAsync(new MateriaPrima("teste", "teste", "un", 100.00));
            await Context.SaveChangesAsync();

            var request = new ProcessoProducaoRequest(DateTime.Now, 1, 1, ciclos, new List<ProcessoProducaoMateriaPrimaRequest> { new(1, quantidadeMateriaPrima) });

            //act & assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => ProcessoProducaoService.AdicionarAsync(request));
            Assert.Equal(errorMessage, exception.Message);
        }

        [Fact]
        public async Task CriarProducaoMateriaPrima()
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();
            await Context.Maquinas.AddAsync(new Maquina("teste", "teste"));
            await Context.Produtos.AddAsync(new Produto("teste", "teste", "un", 10));
            await Context.Formas.AddAsync(new Forma("teste", 1, 10));
            await Context.MateriasPrimas.AddAsync(new MateriaPrima("teste", "teste", "un", 100.00));
            await Context.Producoes.AddAsync(new ProcessoProducao(DateTime.Now, 1, 1, 1, 100));
            await Context.SaveChangesAsync();

            var request = new List<ProcessoProducaoMateriaPrimaRequest> { new(1, 100) };

            var producoesMateriasPrimas = await ProcessoProducaoService.CriarProducoesMateriasPrimas(request, 1);
            var materiaPrimaRetornada = producoesMateriasPrimas.FirstOrDefault();

            var expected = (new List<ProcessoProducaoMateriaPrima> { new(1, 1, 100) }).FirstOrDefault();
            
            Assert.Equal(expected.Quantidade, materiaPrimaRetornada.Quantidade);
            Assert.Equal(expected.MateriaPrimaId, materiaPrimaRetornada.MateriaPrimaId);
            Assert.Equal(expected.ProducaoId, materiaPrimaRetornada.ProducaoId);
        }

        [Fact]
        public async Task CalcularProducao()
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();
            await Context.Maquinas.AddAsync(new Maquina("teste", "teste"));

            var produto = new Produto("teste", "teste", "un", 10);
            await Context.Produtos.AddAsync(produto);

            var forma = new Forma("teste", 1, 10);
            await Context.Formas.AddAsync(forma);

            var materiaPrima = new MateriaPrima("teste", "teste", "un", 100.00);
            await Context.MateriasPrimas.AddAsync(materiaPrima);

            var producaoNova = new ProcessoProducao(DateTime.Now, 1, 1, 1, 100);
            await Context.Producoes.AddAsync(producaoNova);

            var producaoMateriaPrima = new ProcessoProducaoMateriaPrima(1, 1, 100);
            await Context.ProducoesMateriasPrimas.AddAsync(producaoMateriaPrima);
            await Context.SaveChangesAsync();

            await ProcessoProducaoService.CalcularProducao(1);
            var producao = await Context.Producoes.FirstOrDefaultAsync();

            var quantidadeProduzida = (producaoNova.Ciclos * forma.PecasPorCiclo) / produto.PecasPorUnidade;

            var custoTotal = materiaPrima.Preco * producaoMateriaPrima.Quantidade;
            var custoUnitario = custoTotal / quantidadeProduzida;

            Assert.Equal(quantidadeProduzida, producao.QuantidadeProduzida);
            Assert.Equal(custoTotal, producao.CustoTotal);
            Assert.Equal(custoUnitario, producao.CustoUnitario);
        }
    }
}
