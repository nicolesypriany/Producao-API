using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class MaquinaRepository : BaseRepository<Maquina>, IMaquinaRepository
    {
        private readonly ProducaoContext _context;
        public MaquinaRepository(ProducaoContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Maquina> ListarMaquinas()
        {
            return _context.Maquinas.Where(m => m.Ativo == true).ToList();
        }
    }
}
