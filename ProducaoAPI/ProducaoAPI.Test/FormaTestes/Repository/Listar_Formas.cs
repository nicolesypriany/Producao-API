using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Test.FormaTestes.Repository
{
    public class Listar_Formas
    {
        public ProducaoContext Context { get; }
        public IFormaRepository FormaRepository { get; }
        public Listar_Formas()
        {
            var options = new DbContextOptionsBuilder<ProducaoContext>()
               .UseInMemoryDatabase("Teste")
               .Options;

            Context = new ProducaoContext(options);
            FormaRepository = new FormaRepository(Context);
            Context.Produtos.Add(new Produto("Produto", "teste", "un", 10));
        }

        [Fact]
        public async void RetornaErro404AoListarFormasSemFormasCadastradas()
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();

            //act & assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => FormaRepository.ListarTodasFormas());
            Assert.Equal("Nenhuma forma encontrada.", exception.Message);
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public async void RetornaErro404AoListarFormasAtivasSemFormasCadastradas()
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();

            //act & assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => FormaRepository.ListarFormasAtivas());
            Assert.Equal("Nenhuma forma ativa encontrada.", exception.Message);
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public async void RetornaErro404AoListarFormasAtivasSemFormasAtivasCadastradas()
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();

            var forma = new Forma("Forma", 1, 1);
            await Context.Formas.AddAsync(forma);
            forma.Ativo = false;
            List<Forma> lista = [forma];

            //act & assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => FormaRepository.ListarFormasAtivas());
            Assert.Equal("Nenhuma forma ativa encontrada.", exception.Message);
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public async void RetornaFormasAoListarTodasFormas()
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();

            var forma = new Forma("Forma 1", 1, 1);
            await Context.Formas.AddAsync(forma);
            await Context.SaveChangesAsync();
            List<Forma> lista = [forma];

            //act
            var formas = await FormaRepository.ListarTodasFormas();

            //assert
            Assert.Equal(lista, formas);
        }

        [Fact]
        public async void RetornaFormasAtivasAoListarFormasAtivas()
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();

            var forma = new Forma("Forma 3", 1, 1);
            await Context.Formas.AddAsync(forma);

            var forma2 = new Forma("Forma 4", 1, 1);
            await Context.Formas.AddAsync(forma2);

            forma.Ativo = false;
            await Context.SaveChangesAsync();
            List<Forma> lista = [forma2];

            //act
            var formas = await FormaRepository.ListarFormasAtivas();

            //assert
            Assert.Equal(lista, formas);
            Assert.False(forma.Ativo);
            Assert.True(forma2.Ativo);
        }
    }
}