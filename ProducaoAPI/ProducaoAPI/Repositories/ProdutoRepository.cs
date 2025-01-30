using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ProducaoContext _context;
        public ProdutoRepository(ProducaoContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Produto produto)
        {
            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Produto produto)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
        }

        public async Task<Produto> BuscarProdutoPorIdAsync(int id)
        {
            return await _context.Produtos.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Produto>> ListarProdutosAsync()
        {
            return await _context.Produtos.Where(m => m.Ativo == true).ToListAsync();
        }
    }
}
