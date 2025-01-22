using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class ProducaoMateriaPrimaRepository : IProducaoMateriaPrimaRepository
    {
        private readonly ProducaoContext _context;
        public ProducaoMateriaPrimaRepository(ProducaoContext context)
        {
            _context = context;
        }
        public async Task AdicionarAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima)
        {
            await _context.ProducoesMateriasPrimas.AddAsync(producaoMateriaPrima);
            await _context.SaveChangesAsync();
        }
    }
}
