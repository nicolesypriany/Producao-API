using Bogus;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using Xunit.Abstractions;

namespace ProducaoAPI.Test.FormaRepository
{
    [Collection(nameof(ContextoCollection))]
    public class FormaAdicionar
    {
        private readonly ProducaoContext context;

        public FormaAdicionar(ITestOutputHelper output, ContextoFixture fixture)
        {
            context = fixture.Context;
            output.WriteLine(context.GetHashCode().ToString());
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

            var formaRepository = new Repositories.FormaRepository(context);

            //act
            await formaRepository.AdicionarAsync(forma);

            //assert
            var formaIncluida = await formaRepository.BuscarFormaPorIdAsync(forma.Id);
            Assert.NotNull(formaIncluida);
            Assert.Equal(formaIncluida.Nome, forma.Nome);
            Assert.Equal(formaIncluida.PecasPorCiclo, forma.PecasPorCiclo);
            Assert.Equal(formaIncluida.ProdutoId, forma.ProdutoId);
        }

        [Fact]
        public async Task AdicionaFormaComNomeVazio()
        {
            //arrange
            var fakerForma = new Faker<Forma>().CustomInstantiator(f => new Forma(
                   " ",
                   f.Random.Int(1, 5),
                   f.Random.Int(1, 20)
               ));

            var forma = fakerForma.Generate();

            var formaRepository = new Repositories.FormaRepository(context);

            //act & assert
            var exception = await Assert.ThrowsAsync<Exception>(() => formaRepository.AdicionarAsync(forma));
            Assert.Equal("O campo \"Nome\" não pode estar vazio.", exception.Message);
        }

        [Fact]
        public async Task AdicionaFormaComPecasPorCicloInvalida()
        {
            //arrange
            var fakerForma = new Faker<Forma>().CustomInstantiator(f => new Forma(
                    f.Random.Word(),
                    f.Random.Int(1, 5),
                    0
                ));

            var forma = fakerForma.Generate();

            var formaRepository = new FormaRepository(context);

            //act & assert
            var exception = await Assert.ThrowsAsync<Exception>(() => formaRepository.AdicionarAsync(forma));
            Assert.Equal("O número de peças por ciclo deve ser maior do que 0.", exception.Message);
        }

    }
}
