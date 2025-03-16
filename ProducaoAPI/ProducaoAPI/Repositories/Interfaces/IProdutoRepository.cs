using ProducaoAPI.Models;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> ListarProdutosAtivos();
        Task<IEnumerable<Produto>> ListarTodosProdutos();
        Task<Produto> BuscarProdutoPorIdAsync(int id);
        Task AdicionarAsync(Produto produto);
        Task AtualizarAsync(Produto produto);
        Task<IEnumerable<string>> ListarNomes();
    }
}
