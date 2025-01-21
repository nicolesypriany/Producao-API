using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class FormaRepository : IFormaRepository
    {
        private readonly ProducaoContext _context;
        public FormaRepository(ProducaoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Forma>> ListarFormasAsync()
        {
            return await _context.Formas.Where(m => m.Ativo == true).Include(f => f.Maquinas).Include(f => f.Produto).ToListAsync();
        }

        public async Task<Forma> BuscarFormaPorIdAsync(int id)
        {
            return await _context.Formas.Include(f => f.Maquinas).Include(f => f.Produto).FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task AdicionarAsync(Forma forma)
        {
            await _context.Formas.AddAsync(forma);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Forma forma)
        {
            _context.Formas.Update(forma);
            await _context.SaveChangesAsync();
        }
    }
}