using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
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

        public async Task<IEnumerable<Maquina>> ListarMaquinasAtivas()
        {
            var maquinas = await _context.Maquinas
            .Where(m => m.Ativo == true)
            .ToListAsync();

            if (maquinas == null || maquinas.Count == 0) throw new NotFoundException("Nenhuma máquina ativa encontrada.");
            return maquinas;
        }

        public async Task<IEnumerable<Maquina>> ListarTodasMaquinas()
        {
            var maquinas = await _context.Maquinas
            .ToListAsync();

            if (maquinas == null || maquinas.Count == 0) throw new NotFoundException("Nenhuma máquina encontrada.");
            return maquinas;
        }

        public async Task<Maquina> BuscarMaquinaPorIdAsync(int id)
        {
            var maquina = await _context.Maquinas
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maquina == null) throw new NotFoundException("ID da máquina não encontrado.");
            return maquina;
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

        public async Task<IEnumerable<string>> ListarNomes()
        {
            return await _context.Maquinas
                .Select(m => m.Nome)
                .ToListAsync();
        }
    }
}