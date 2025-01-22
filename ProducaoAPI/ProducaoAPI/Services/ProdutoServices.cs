using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;

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

        public Task<IEnumerable<Produto>> ListarProdutosAsync() => _produtoRepository.ListarProdutosAsync();

        public Task<Produto> BuscarProdutoPorIdAsync(int id) => _produtoRepository.BuscarProdutoPorIdAsync(id);

        public Task AdicionarAsync(Produto produto) => _produtoRepository.AdicionarAsync(produto);

        public Task AtualizarAsync(Produto produto) => _produtoRepository.AtualizarAsync(produto);
    }
}
