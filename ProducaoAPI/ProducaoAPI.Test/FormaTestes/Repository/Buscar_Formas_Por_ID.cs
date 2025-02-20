using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Test.FormaTestes.Repository
{
    public class Buscar_Formas_Por_ID
    {
        public ProducaoContext Context { get; }
        public IFormaRepository FormaRepository { get; }
        public Buscar_Formas_Por_ID()
        {
            var options = new DbContextOptionsBuilder<ProducaoContext>()
               .UseInMemoryDatabase("Teste")
               .Options;

            Context = new ProducaoContext(options);
            FormaRepository = new FormaRepository(Context);
            Context.Produtos.Add(new Produto("Produto", "teste", "un", 10));
        }

        [Fact]
        public async void RetornaErro404AoBuscarFormaPorIDInexistente()
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();

            //act & assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => FormaRepository.BuscarFormaPorIdAsync(1));
            Assert.Equal("ID da forma não encontrado.", exception.Message);
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public async void SucessoAoBuscarFormaPorIDExistente()
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();

            var forma = new Forma("Forma", 1, 10);
            await Context.Formas.AddAsync(forma);
            await Context.SaveChangesAsync();

            //act
            var formaBuscada = await FormaRepository.BuscarFormaPorIdAsync(forma.Id);

            //assert
            Assert.Equal(forma, formaBuscada);
            Assert.NotNull(formaBuscada);
        }
    }
}
