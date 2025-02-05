using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Services.Interfaces
{
    public interface IProdutoService
    {
        Task<IEnumerable<Produto>> ListarProdutosAsync();
        Task<Produto> BuscarProdutoPorIdAsync(int id);
        Task AdicionarAsync(Produto produto);
        Task AtualizarAsync(Produto produto);
        ProdutoResponse EntityToResponse(Produto produto);
        ICollection<ProdutoResponse> EntityListToResponseList(IEnumerable<Produto> produto);
        Task ValidarDados(ProdutoRequest request);
    }
}
