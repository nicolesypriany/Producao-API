using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class ProcessoProducaoRepository : BaseRepository<ProcessoProducao>, IProcessoProducaoRepository
    {
        private readonly ProducaoContext _context;
        public ProcessoProducaoRepository(ProducaoContext context) : base(context)
        {
            _context = context;
        }

        public ProcessoProducao BuscarProducaoPorId(int id)
        {
            return _context.Producoes
               .Include(p => p.ProducaoMateriasPrimas)
               .ThenInclude(p => p.MateriaPrima)
               .Where(m => m.Ativo == true)
               .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<ProcessoProducao> ListarProducoes()
        {
            return _context.Producoes
                .Include(p => p.ProducaoMateriasPrimas)
                .ThenInclude(p => p.MateriaPrima)
                .Where(m => m.Ativo == true)
                .ToList();
        }
    }
}
