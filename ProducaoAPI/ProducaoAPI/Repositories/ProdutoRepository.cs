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

        public async Task<IEnumerable<Produto>> ListarProdutosAtivos()
        {
            try
            {
                var produtos = await _context.Produtos
                    .Where(m => m.Ativo == true)
                    .ToListAsync();

                if (produtos == null || produtos.Count == 0) throw new NullReferenceException("Nenhum produto ativo.");
                return produtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Produto>> ListarTodosProdutos()
        {
            try
            {
                var produtos = await _context.Produtos
                    .ToListAsync();

                if (produtos == null || produtos.Count == 0) throw new NullReferenceException("Nenhum produto encontrado.");
                return produtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Produto> BuscarProdutoPorIdAsync(int id)
        {
            try
            {
                var produto = await _context.Produtos
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (produto == null) throw new NullReferenceException("ID do produto não encontrado.");
                return produto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AdicionarAsync(Produto produto)
        {
            try
            {
                await _context.Produtos.AddAsync(produto);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AtualizarAsync(Produto produto)
        {
            try
            {
                _context.Produtos.Update(produto);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
