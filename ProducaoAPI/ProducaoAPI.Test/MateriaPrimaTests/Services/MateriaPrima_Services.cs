using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Services;
using ProducaoAPI.Services.Interfaces;


namespace ProducaoAPI.Test.MateriaPrimaTests.Services
{
    public class MateriaPrima_Services
    {
        public ProducaoContext Context { get; }
        public IMateriaPrimaRepository MateriaPrimaRepository { get; }
        public IMateriaPrimaService MateriaPrimaService { get; }
        public MateriaPrima_Services()
        {
            var options = new DbContextOptionsBuilder<ProducaoContext>()
                           .UseInMemoryDatabase("Teste")
                           .Options;

            Context = new ProducaoContext(options);
            MateriaPrimaRepository = new MateriaPrimaRepository(Context);
            MateriaPrimaService = new MateriaPrimaServices(MateriaPrimaRepository);
        }

        [Theory]
        [InlineData("teste", "teste", "un", 30, "Já existe um cadastro com este nome!")]
        [InlineData(" ", "teste", "un", 30, "O campo 'Nome' não pode estar vazio.")]
        [InlineData("teste 1", " ", "un", 30, "O campo 'Fornecedor' não pode estar vazio.")]
        [InlineData("teste 2", "teste", " ", 30, "O campo 'Unidade' não pode estar vazio.")]
        [InlineData("teste 3", "teste", "unidade", 30, "A sigla da unidade não pode ter mais de 5 caracteres.")]
        [InlineData("teste 4", "teste", "un", 0, "O preço não pode ser igual ou menor que 0.")]
        public async Task RetornaErroAoValidarDados(string nome, string fornecedor, string unidade, double preco, string errorMessage)
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();

            await Context.MateriasPrimas.AddAsync(new MateriaPrima("teste", "teste", "un", 10.00));
            await Context.SaveChangesAsync();

            var materiaPrimaRequest = new MateriaPrimaRequest(nome, fornecedor, unidade, preco);

            //act & assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => MateriaPrimaService.AdicionarAsync(materiaPrimaRequest));
            Assert.Equal(errorMessage, exception.Message);
        }
    }
}
