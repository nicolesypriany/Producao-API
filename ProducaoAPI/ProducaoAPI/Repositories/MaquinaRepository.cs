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

        public async Task<IEnumerable<Maquina>> ListarMaquinasAsync()
        {
            var maquinas = await _context.Maquinas
                .Where(m => m.Ativo == true)
                .ToListAsync();

            if (maquinas == null || maquinas.Count == 0) throw new NullReferenceException("Nenhuma máquina encontrada.");
            return maquinas;
        }

        public async Task<Maquina> BuscarMaquinaPorIdAsync(int id)
        {
            try
            {
                var maquina = await _context.Maquinas
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (maquina == null) throw new NullReferenceException("ID da máquina não encontrado.");
                return maquina;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AdicionarAsync(Maquina maquina)
        {
            try
            {
                await _context.Maquinas.AddAsync(maquina);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                throw new Exception(ex.Message);
            }
        }
    }
}
