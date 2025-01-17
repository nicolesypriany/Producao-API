using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class MateriaPrimaRepository : BaseRepository<MateriaPrima>, IMateriaPrimaRepository
    {
        private readonly ProducaoContext _context;
        public MateriaPrimaRepository(ProducaoContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<MateriaPrima> ListarMateriasPrimas()
        {
            return _context.MateriasPrimas.Where(m => m.Ativo == true).ToList();
        }
    }
}
