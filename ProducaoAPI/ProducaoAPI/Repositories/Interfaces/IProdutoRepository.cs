using ProducaoAPI.Models;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IProdutoRepository : IBaseRepository<Produto>
    {
        IEnumerable<Produto> ListarProdutos();
    }
}
