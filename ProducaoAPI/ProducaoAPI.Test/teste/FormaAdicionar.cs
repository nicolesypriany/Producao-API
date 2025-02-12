using Bogus;
using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories;

namespace ProducaoAPI.Test.teste
{
    public class FormaAdicionar
    {
        private readonly ProducaoContext _context;
        private readonly ProdutoRepository _produtoRepository;
        private readonly FormaRepository _formaRepository;


        public FormaAdicionar()
        {
            var options = new DbContextOptionsBuilder<ProducaoContext>()
                .UseInMemoryDatabase(databaseName: "BancoTeste")
                .Options;

            _context = new ProducaoContext(options);
            _produtoRepository = new ProdutoRepository(_context);
            _formaRepository = new FormaRepository(_context, _produtoRepository);
        }

        [Fact]
        public async void AdicionaFormaComDadosValidos()
        {
            //arrange
            var fakerForma = new Faker<Forma>().CustomInstantiator(f => new Forma(
                    f.Random.Word(),
                    f.Random.Int(1, 5),
                    f.Random.Int(1, 20)
                ));

            var forma = fakerForma.Generate();

            //act
            await _formaRepository.AdicionarAsync(forma);
            var formaIncluida = await _context.Formas.FindAsync(forma.Id);

            //assert
            Assert.NotNull(formaIncluida);
            Assert.Equal(formaIncluida.Nome, forma.Nome);
            Assert.Equal(formaIncluida.PecasPorCiclo, forma.PecasPorCiclo);
            Assert.Equal(formaIncluida.ProdutoId, forma.ProdutoId);
        }

        //[Fact]
        //public async Task AdicionaFormaComNomeVazio()
        //{
        //    //arrange
        //    var fakerForma = new Faker<Forma>().CustomInstantiator(f => new Forma(
        //           " ",
        //           f.Random.Int(1, 5),
        //           f.Random.Int(1, 20)
        //       ));

        //    var forma = fakerForma.Generate();

        //    //var produtoRepository = new ProdutoRepository(_context);
        //    //var formaRepository = new FormaRepository(_context, produtoRepository);

        //    //act & assert
        //    var exception = await Assert.ThrowsAsync<Exception>(() => _formaRepository.AdicionarAsync(forma));
        //    Assert.Equal("O campo \"Nome\" não pode estar vazio.", exception.Message);
        //}

        //[Fact]
        //public async Task AdicionaFormaComPecasPorCicloInvalida()
        //{
        //    //arrange
        //    var fakerForma = new Faker<Forma>().CustomInstantiator(f => new Forma(
        //            f.Random.Word(),
        //            f.Random.Int(1, 5),
        //            0
        //        ));

        //    var forma = fakerForma.Generate();

        //    var produtoRepository = new ProdutoRepository(_context);
        //    var formaRepository = new FormaRepository(_context, produtoRepository);

        //    //act & assert
        //    var exception = await Assert.ThrowsAsync<Exception>(() => formaRepository.AdicionarAsync(forma));
        //    Assert.Equal("O número de peças por ciclo deve ser maior do que 0.", exception.Message);
        //}
    }
}
