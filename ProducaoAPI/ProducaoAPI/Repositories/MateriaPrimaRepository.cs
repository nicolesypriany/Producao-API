using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class MateriaPrimaRepository : IMateriaPrimaRepository
    {
        private readonly ProducaoContext _context;
        public MateriaPrimaRepository(ProducaoContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(MateriaPrima materiaPrima)
        {
            await _context.MateriasPrimas.AddAsync(materiaPrima);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(MateriaPrima materiaPrima)
        {
            _context.MateriasPrimas.Update(materiaPrima);
            await _context.SaveChangesAsync();
        }

        public async Task<MateriaPrima> BuscarMateriaPorIdAsync(int id)
        {
            return await _context.MateriasPrimas.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<MateriaPrima>> ListarMateriasAsync()
        {
            return await _context.MateriasPrimas.Where(m => m.Ativo == true).ToListAsync();
        }
    }
}
