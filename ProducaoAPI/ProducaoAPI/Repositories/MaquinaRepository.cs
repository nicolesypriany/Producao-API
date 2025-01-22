using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class MaquinaRepository : IMaquinaRepository
    {
        private readonly ProducaoContext _context;
        public MaquinaRepository(ProducaoContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Maquina maquina)
        {
            await _context.Maquinas.AddAsync(maquina);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Maquina maquina)
        {
            _context.Maquinas.Update(maquina);
            await _context.SaveChangesAsync();
        }

        public async Task<Maquina> BuscarMaquinaPorIdAsync(int id)
        {
            return await _context.Maquinas.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Maquina>> ListarMaquinasAsync()
        {
            return await _context.Maquinas.Where(m => m.Ativo == true).ToListAsync();
        }
    }
}
