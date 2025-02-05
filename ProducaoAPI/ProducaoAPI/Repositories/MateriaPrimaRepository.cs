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

        public async Task<IEnumerable<MateriaPrima>> ListarMateriasAsync()
        {
            try
            {
                var materiasPrimas = await _context.MateriasPrimas
                    .Where(m => m.Ativo == true)
                    .ToListAsync();

                if (materiasPrimas == null || materiasPrimas.Count == 0) throw new NullReferenceException("Nenhuma matéria-prima encontrada.");
                return materiasPrimas;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MateriaPrima> BuscarMateriaPorIdAsync(int id)
        {
            try
            {
                var materiaPrima = await _context.MateriasPrimas
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (materiaPrima == null) throw new NullReferenceException("ID da matéria-prima não encontrado.");
                return materiaPrima;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AdicionarAsync(MateriaPrima materiaPrima)
        {
            try
            {
                await _context.MateriasPrimas.AddAsync(materiaPrima);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AtualizarAsync(MateriaPrima materiaPrima)
        {
            try
            {
                _context.MateriasPrimas.Update(materiaPrima);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
