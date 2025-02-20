using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Test.FormaTestes.Repository
{
    public class FormasAdicionar
    {
        public ProducaoContext Context { get; }
        public IFormaRepository FormaRepository { get; }
        public IProdutoRepository ProdutoRepository { get; }

        public FormasAdicionar()
        {
            var options = new DbContextOptionsBuilder<ProducaoContext>()
               .UseInMemoryDatabase("Teste")
               .Options;

            Context = new ProducaoContext(options);
            ProdutoRepository = new ProdutoRepository(Context);
            FormaRepository = new FormaRepository(Context, ProdutoRepository);
            Context.Produtos.Add(new Produto("Produto", "teste", "un", 10));
        }

        [Fact]
        public async void AdicionarForma()
        {
            //arrange
            Context.Database.EnsureDeleted();
            var forma = new Forma("teste", 1, 10);

            //act
            await FormaRepository.AdicionarAsync(forma);
            var formaAdicionada = await Context.Formas.FirstOrDefaultAsync(f => f.Id == forma.Id);

            //assert
            Assert.Equal(forma, formaAdicionada);
        }
    }
}
