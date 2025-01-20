using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
    {
        private readonly ProducaoContext _context;
        public ProdutoRepository(ProducaoContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<Produto> ListarProdutos()
        {
            return _context.Produtos.Where(m => m.Ativo == true).ToList();
        }
    }
}
