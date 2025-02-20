using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Test.FormaTestes.Repository
{
    public class Atualizar_Forma
    {
        public ProducaoContext Context { get; }
        public IFormaRepository FormaRepository { get; }
        public Atualizar_Forma()
        {
            var options = new DbContextOptionsBuilder<ProducaoContext>()
               .UseInMemoryDatabase("Teste")
               .Options;

            Context = new ProducaoContext(options);
            FormaRepository = new FormaRepository(Context);
            Context.Produtos.Add(new Produto("Produto", "teste", "un", 10));
        }

        [Fact]
        public async void AtualizarForma()
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();

            var forma = new Forma("teste", 1, 10);
            await Context.Formas.AddAsync(forma);
            await Context.SaveChangesAsync();

            //act
            forma.Nome = "forma";
            forma.ProdutoId = 2;
            forma.PecasPorCiclo = 15;
            forma.Ativo = false;

            await FormaRepository.AtualizarAsync(forma);
            var formaAtualizada = await Context.Formas.FindAsync(forma.Id);

            //assert
            Assert.Equal("forma", formaAtualizada.Nome);
            Assert.Equal(2, formaAtualizada.ProdutoId);
            Assert.Equal(15, formaAtualizada.PecasPorCiclo);
            Assert.False(forma.Ativo);
        }
    }
}