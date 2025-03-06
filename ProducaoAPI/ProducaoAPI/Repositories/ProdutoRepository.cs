using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
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

        public async Task<IEnumerable<Produto>> ListarProdutosAtivos()
        {
            var produtos = await _context.Produtos
                .Where(m => m.Ativo == true)
                .ToListAsync();

            if (produtos == null || produtos.Count == 0) throw new NotFoundException("Nenhum produto ativo.");
            return produtos;
        }

        public async Task<IEnumerable<Produto>> ListarTodosProdutos()
        {
            var produtos = await _context.Produtos
                .ToListAsync();

            if (produtos == null || produtos.Count == 0) throw new NotFoundException("Nenhum produto encontrado.");
            return produtos;
        }

        public async Task<Produto> BuscarProdutoPorIdAsync(int id)
        {
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (produto == null) throw new NotFoundException("ID do produto não encontrado.");
            return produto;
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

        public async Task<IEnumerable<string>> ListarNomes()
        {
            return await _context.Produtos
                .Select(p => p.Nome)
                .ToListAsync();
        }
    }
}
