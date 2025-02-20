using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Services;
using ProducaoAPI.Services.Interfaces;
using System.Runtime.Intrinsics.X86;
using Xunit.Sdk;

namespace ProducaoAPI.Test.FormaTestes.Services
{
    public class FormaAdicionar
    {
        public ProducaoContext Context { get; }
        public IFormaService FormaService { get; }
        public IMaquinaService MaquinaService { get; }
        public IProdutoService ProdutoService { get; }
        public IMaquinaRepository MaquinaRepository { get; }
        public IFormaRepository FormaRepository { get; }
        public IProdutoRepository ProdutoRepository { get; }
        public FormaAdicionar()
        {
            var options = new DbContextOptionsBuilder<ProducaoContext>()
                           .UseInMemoryDatabase("Teste")
                           .Options;

            Context = new ProducaoContext(options);
            ProdutoRepository = new ProdutoRepository(Context);
            FormaRepository = new FormaRepository(Context, ProdutoRepository);
            MaquinaRepository = new MaquinaRepository(Context);
            ProdutoService = new ProdutoServices(ProdutoRepository);
            MaquinaService = new MaquinaServices(MaquinaRepository);
            FormaService = new FormaServices(FormaRepository, MaquinaService, ProdutoService);
        }

        [Theory]
        [InlineData("", "O campo \"Nome\" não pode estar vazio.")]
        [InlineData(" ", "O campo \"Nome\" não pode estar vazio.")]
        [InlineData("Teste", "Já existe uma forma com este nome!")]
        public async Task ValidarDadosComNomeVazioOuEmBrancoOuDuplicado(string name, string errorMessage)
        {
            //arrange
            Context.Produtos.Add(new Produto("Produto", "teste", "un", 10));
            Context.SaveChanges();

            Context.Formas.Add(new Forma("Teste", 1, 100));
            Context.SaveChanges();
            var formaRequest = new FormaRequest(name, 1, 100, new List<FormaMaquinaRequest>(1));

            //act & assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => FormaService.ValidarDadosParaCadastrar(formaRequest));
            Assert.Equal(errorMessage, exception.Message);
        }

        [Fact]
        public async Task ValidarDadosComNumeroDePecasNegativo()
        {
            //arrange
            Context.Produtos.Add(new Produto("Produto", "teste", "un", 10));
            Context.SaveChanges();

            Context.Formas.Add(new Forma("Teste", 1, 100));
            Context.SaveChanges();
            var formaRequest = new FormaRequest("Forma", 1, -1, new List<FormaMaquinaRequest>(1));

            //act & assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => FormaService.ValidarDadosParaCadastrar(formaRequest));
            Assert.Equal("O número de peças por ciclo deve ser maior do que 0.", exception.Message);
        }
    }
}
