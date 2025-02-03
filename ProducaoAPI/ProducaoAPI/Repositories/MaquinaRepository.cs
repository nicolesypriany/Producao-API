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
            try
            {
                if (string.IsNullOrWhiteSpace(maquina.Nome)) throw new ArgumentException("O campo \"Nome\" não pode estar vazio");

                if (string.IsNullOrWhiteSpace(maquina.Marca)) throw new ArgumentException("O campo \"Marca\" não pode estar vazio");

                await _context.Maquinas.AddAsync(maquina);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message + ex.Message);
            }
        }

        public async Task AtualizarAsync(Maquina maquina)
        {
            try
            {
                _context.Maquinas.Update(maquina);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro: {ex.Message}");
            }
        }

        public async Task<Maquina> BuscarMaquinaPorIdAsync(int id)
        {
            try
            {
                return await _context.Maquinas.FirstOrDefaultAsync(m => m.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Maquina>> ListarMaquinasAsync()
        {
            return await _context.Maquinas.Where(m => m.Ativo == true).ToListAsync();
        }
    }
}
