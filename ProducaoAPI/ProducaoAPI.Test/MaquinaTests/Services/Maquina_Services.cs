using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Services;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Test.MaquinaTests.Services
{
    public class Maquina_Services
    {
        public ProducaoContext Context { get; }
        public IMaquinaService MaquinaService { get; }
        public IMaquinaRepository MaquinaRepository { get; }
        public Maquina_Services()
        {
            var options = new DbContextOptionsBuilder<ProducaoContext>()
                           .UseInMemoryDatabase("Teste")
                           .Options;

            Context = new ProducaoContext(options);
            MaquinaRepository = new MaquinaRepository(Context);
            MaquinaService = new MaquinaServices(MaquinaRepository);
        }

        [Theory]
        [InlineData("", "teste", "O campo 'Nome' não pode estar vazio")]
        [InlineData(" ", "teste", "O campo 'Nome' não pode estar vazio")]
        [InlineData("Teste", "teste", "Já existe um cadastro com este nome!")]
        [InlineData("teste 1", "", "O campo 'Marca' não pode estar vazio")]
        [InlineData("teste 2", " ", "O campo 'Marca' não pode estar vazio")]
        public async Task ValidarDadosComNomeEMarcaVazioOuEmBrancoOuDuplicado(string name, string marca, string errorMessage)
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();

            await Context.Maquinas.AddAsync(new Maquina("Teste", "Teste"));
            await Context.SaveChangesAsync();
            var maquinaRequest = new MaquinaRequest(name, marca);

            //act & assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => MaquinaService.AdicionarAsync(maquinaRequest));
            Assert.Equal(errorMessage, exception.Message);
        }

        [Fact]
        public async Task InativarMaquina()
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();
            var maquina = new Maquina("teste", "teste");
            await Context.Maquinas.AddAsync(maquina);
            await Context.SaveChangesAsync();

            //act
            await MaquinaService.InativarMaquina(maquina.Id);

            //assert
            Assert.False(maquina.Ativo);
        }
    }
}
