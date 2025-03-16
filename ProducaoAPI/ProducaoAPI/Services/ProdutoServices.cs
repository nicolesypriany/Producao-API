using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;
using ProducaoAPI.Validations;

namespace ProducaoAPI.Services
{
    public class ProdutoServices : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        public ProdutoServices(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public ProdutoResponse EntityToResponse(Produto produto)
        {
            return new ProdutoResponse(produto.Id, produto.Nome, produto.Medidas, produto.Unidade, produto.PecasPorUnidade, produto.Ativo);
        }

        public ICollection<ProdutoResponse> EntityListToResponseList(IEnumerable<Produto> produto)
        {
            return produto.Select(f => EntityToResponse(f)).ToList();
        }

        public Task<IEnumerable<Produto>> ListarProdutosAtivos() => _produtoRepository.ListarProdutosAtivos();

        public Task<IEnumerable<Produto>> ListarTodosProdutos() => _produtoRepository.ListarTodosProdutos();

        public Task<Produto> BuscarProdutoPorIdAsync(int id) => _produtoRepository.BuscarProdutoPorIdAsync(id);

        public async Task<Produto> AdicionarAsync(ProdutoRequest request)
        {
            await ValidarRequest(true, request);
            var produto = new Produto(request.Nome, request.Medidas, request.Unidade, request.PecasPorUnidade);
            await _produtoRepository.AdicionarAsync(produto);
            return produto;
        }

        public async Task<Produto> AtualizarAsync(int id, ProdutoRequest request)
        {
            var produto = await _produtoRepository.BuscarProdutoPorIdAsync(id);
            await ValidarRequest(false, request, produto.Nome);

            produto.Nome = request.Nome;
            produto.Medidas = request.Medidas;
            produto.Unidade = request.Unidade;
            produto.PecasPorUnidade = request.PecasPorUnidade;

            await _produtoRepository.AtualizarAsync(produto);
            return produto;
        }

        public async Task<Produto> InativarProduto(int id)
        {
            var produto = await BuscarProdutoPorIdAsync(id);
            produto.Ativo = false;
            await _produtoRepository.AtualizarAsync(produto);
            return produto;
        }

        private async Task ValidarRequest(bool Cadastrar, ProdutoRequest request, string nomeAtual = "")
        {
            var nomeProdutos = await _produtoRepository.ListarNomes();

            ValidarCampos.Nome(Cadastrar, nomeProdutos, request.Nome, nomeAtual);
            ValidarCampos.String(request.Medidas, "Medidas");
            ValidarCampos.String(request.Unidade, "Unidade");
            ValidarCampos.Unidade(request.Unidade);
            ValidarCampos.Inteiro(request.PecasPorUnidade, "Peças por Unidade");
        }
    }
}
