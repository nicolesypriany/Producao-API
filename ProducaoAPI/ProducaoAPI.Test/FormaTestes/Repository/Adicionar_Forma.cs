using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Test.FormaTestes.Repository
{
    public class Adicionar_Forma
    {
        public ProducaoContext Context { get; }
        public IFormaRepository FormaRepository { get; }
        public Adicionar_Forma()
        {
            var options = new DbContextOptionsBuilder<ProducaoContext>()
               .UseInMemoryDatabase("Teste")
               .Options;

            Context = new ProducaoContext(options);
            FormaRepository = new FormaRepository(Context);
            Context.Produtos.Add(new Produto("Produto", "teste", "un", 10));
        }

        [Fact]
        public async void AdicionarForma()
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();
            var forma = new Forma("teste", 1, 10);

            //act
            await FormaRepository.AdicionarAsync(forma);
            var formaAdicionada = await Context.Formas.FirstOrDefaultAsync(f => f.Id == forma.Id);

            //assert
            Assert.Equal(forma, formaAdicionada);
        }
    }
}
