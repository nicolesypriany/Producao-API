using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Services;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Test.FormaTestes.Services
{
    public class Forma_Services
    {
        public ProducaoContext Context { get; }
        public IFormaService FormaService { get; }
        public IMaquinaService MaquinaService { get; }
        public IProdutoService ProdutoService { get; }
        public IMaquinaRepository MaquinaRepository { get; }
        public IFormaRepository FormaRepository { get; }
        public IProdutoRepository ProdutoRepository { get; }
        public Forma_Services()
        {
            var options = new DbContextOptionsBuilder<ProducaoContext>()
                           .UseInMemoryDatabase("Teste")
                           .Options;

            Context = new ProducaoContext(options);
            ProdutoRepository = new ProdutoRepository(Context);
            FormaRepository = new FormaRepository(Context);
            MaquinaRepository = new MaquinaRepository(Context);
            ProdutoService = new ProdutoServices(ProdutoRepository);
            MaquinaService = new MaquinaServices(MaquinaRepository);
            FormaService = new FormaServices(FormaRepository, MaquinaService, ProdutoService);
        }

        [Theory]
        [InlineData("", "O campo 'Nome' não pode estar vazio.")]
        [InlineData(" ", "O campo 'Nome' não pode estar vazio.")]
        [InlineData("Teste", "Já existe um cadastro com este nome!")]
        public async Task ValidarDadosComNomeVazioOuEmBrancoOuDuplicado(string name, string errorMessage)
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();
            await Context.Produtos.AddAsync(new Produto("Produto", "teste", "un", 10));
            await Context.Formas.AddAsync(new Forma("Teste", 1, 100));
            await Context.SaveChangesAsync();
            var formaRequest = new FormaRequest(name, 1, 100, new List<FormaMaquinaRequest>(1));

            //act & assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => FormaService.AdicionarAsync(formaRequest));
            Assert.Equal(errorMessage, exception.Message);
        }

        [Fact]
        public async Task ValidarDadosComNumeroDePecasNegativo()
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();
            await Context.Produtos.AddAsync(new Produto("Produto", "teste", "un", 10));
            await Context.Formas.AddAsync(new Forma("Teste", 1, 100));
            await Context.SaveChangesAsync();
            var formaRequest = new FormaRequest("Forma", 1, -1, new List<FormaMaquinaRequest>(1));

            //act & assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => FormaService.AdicionarAsync(formaRequest));
            Assert.Equal("O número de peças por ciclo deve ser maior do que 0.", exception.Message);
        }

        [Fact]
        public async Task InativarForma()
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();
            await Context.Produtos.AddAsync(new Produto("Produto", "teste", "un", 10));
            var forma = new Forma("Teste", 1, 100);
            await Context.Formas.AddAsync(forma);
            await Context.SaveChangesAsync();

            //act
            await FormaService.InativarForma(forma.Id);

            //assert
            Assert.False(forma.Ativo);
        }
    }
}
