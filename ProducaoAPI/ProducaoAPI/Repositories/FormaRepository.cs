using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class FormaRepository : BaseRepository<Forma>, IFormaRepository
    {
        private readonly ProducaoContext _context;
        public FormaRepository(ProducaoContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<Forma> ListarFormas()
        {
            return _context.Formas.Where(m => m.Ativo == true).Include(f => f.Maquinas).Include(f => f.Produto).ToList();
        }
        public Forma BuscarFormaPorId(int id)
        {
            return _context.Formas.Include(f => f.Maquinas).Include(f => f.Produto).FirstOrDefault(f => f.Id == id);
        }
    }
}
