using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class ProcessoProducaoRepository : IProcessoProducaoRepository
    {
        private readonly ProducaoContext _context;
        public ProcessoProducaoRepository(ProducaoContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(ProcessoProducao producao)
        {
            await _context.Producoes.AddAsync(producao);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(ProcessoProducao producao)
        {
            _context.Producoes.Update(producao);
            await _context.SaveChangesAsync();
        }

        public async Task<ProcessoProducao> BuscarProducaoPorIdAsync(int id)
        {
            return await _context.Producoes 
               .Include(p => p.ProducaoMateriasPrimas)
               .ThenInclude(p => p.MateriaPrima)
               .Where(m => m.Ativo == true)
               .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<ProcessoProducao>> ListarProducoesAsync()
        {
            return await _context.Producoes
                .Include(p => p.ProducaoMateriasPrimas)
                .ThenInclude(p => p.MateriaPrima)
                .Where(m => m.Ativo == true)
                .ToListAsync();
        }
    }
}
