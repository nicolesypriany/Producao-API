using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Services;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Test.ProdutoTests.Services
{
    public class Produto_Services
    {
        public ProducaoContext Context { get; }
        public IProdutoRepository ProdutoRepository { get; }
        public IProdutoService ProdutoService { get; }

        public Produto_Services()
        {
            var options = new DbContextOptionsBuilder<ProducaoContext>()
                           .UseInMemoryDatabase("Teste")
                           .Options;

            Context = new ProducaoContext(options);
            ProdutoRepository = new ProdutoRepository(Context);
            ProdutoService = new ProdutoServices(ProdutoRepository);
        }

        [Theory]
        [InlineData(" ", "teste", "un", 10, "O campo 'Nome' não pode estar vazio.")]
        [InlineData("", "teste", "un", 10, "O campo 'Nome' não pode estar vazio.")]
        [InlineData("teste 2", " ", "un", 10, "O campo 'Medidas' não pode estar vazio.")]
        [InlineData("teste 3", "teste", " ", 10, "O campo 'Unidade' não pode estar vazio.")]
        [InlineData("teste 3", "teste", "unidade", 10, "A sigla da unidade não pode ter mais de 5 caracteres.")]
        [InlineData("teste 4", "teste", "un", 0, "O número de 'Peças por Unidade' deve ser maior do que 0.")]
        public async Task RetornaErroAoValidarDados(string nome, string medidas, string unidade, int pecas, string errorMessage)
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();

            await Context.Produtos.AddAsync(new Produto("Teste", "Teste", "un", 10));
            await Context.SaveChangesAsync();

            var produtoRequest = new ProdutoRequest(nome, medidas, unidade, pecas);

            //act & assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => ProdutoService.AdicionarAsync(produtoRequest));
            Assert.Equal(errorMessage, exception.Message);
        }

        [Fact]
        public async Task InativarProduto()
        {
            //arrange
            await Context.Database.EnsureDeletedAsync();
            var produto = new Produto("teste", "teste", "un", 10);
            await Context.Produtos.AddAsync(produto);
            await Context.SaveChangesAsync();

            //act
            await ProdutoService.InativarProduto(produto.Id);

            //assert
            Assert.False(produto.Ativo);
        }
    }
}
