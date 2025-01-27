using ProducaoAPI.Models;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> ListarProdutosAsync();
        Task<Produto> BuscarProdutoPorIdAsync(int id);
        Task AdicionarAsync(Produto produto);
        Task AtualizarAsync(Produto produto);
    }
}
