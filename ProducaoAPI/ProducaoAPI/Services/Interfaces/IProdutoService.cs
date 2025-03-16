using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Services.Interfaces
{
    public interface IProdutoService
    {
        Task<IEnumerable<Produto>> ListarProdutosAtivos();
        Task<IEnumerable<Produto>> ListarTodosProdutos();
        Task<Produto> BuscarProdutoPorIdAsync(int id);
        Task<Produto> AdicionarAsync(ProdutoRequest request);
        Task<Produto> AtualizarAsync(int id, ProdutoRequest request);
        Task<Produto> InativarProduto(int id);
        ProdutoResponse EntityToResponse(Produto produto);
        ICollection<ProdutoResponse> EntityListToResponseList(IEnumerable<Produto> produto);
    }
}
