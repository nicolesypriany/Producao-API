using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Repositories;
using Xunit.Abstractions;

namespace ProducaoAPI.Test.teste
{
    public class FormaListar
    {
        private readonly ProducaoContext _context;
        public FormaListar()
        {
            var options = new DbContextOptionsBuilder<ProducaoContext>()
                .UseInMemoryDatabase(databaseName: "BancoTeste")
                .Options;

            _context = new ProducaoContext(options);
        }

        //[Fact]
        //public async void RetornaNuloQuandoIdInexistente()
        //{
        //    //arrange
        //    var formaRepository = new Repositories.FormaRepository(context);

        //    //act & assert
        //    var exception = await Assert.ThrowsAsync<Exception>(() => formaRepository.ListarFormasAsync());
        //    Assert.Equal("ID da forma não encontrado!", exception.Message);
        //}

        [Fact]
        public async void RetornaIDQuandoIdExistente()
        {
            //arrange
            var produtoRepository = new ProdutoRepository(_context);
            var formaRepository = new Repositories.FormaRepository(_context, produtoRepository);

            //act & assert
            var formaRecuperada = await formaRepository.BuscarFormaPorIdAsync(1);
            Assert.NotNull(formaRecuperada);
            Assert.Equal(1, formaRecuperada.Id);
        }
    }
}
