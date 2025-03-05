using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class FormaRepository : IFormaRepository
    {
        private readonly ProducaoContext _context;
        public FormaRepository(ProducaoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Forma>> ListarFormasAtivas()
        {
            var formas = await _context.Formas
                .Where(m => m.Ativo == true)
                .Include(f => f.Maquinas)
                .Include(f => f.Produto)
                .ToListAsync();

            if (formas == null || formas.Count == 0) throw new NotFoundException("Nenhuma forma ativa encontrada.");
            return formas;
        }

        public async Task<IEnumerable<Forma>> ListarTodasFormas()
        {
            var formas = await _context.Formas
                .Include(f => f.Maquinas)
                .Include(f => f.Produto)
                .ToListAsync();

            if (formas == null || formas.Count == 0) throw new NotFoundException("Nenhuma forma encontrada.");
            return formas;
        }

        public async Task<Forma> BuscarFormaPorIdAsync(int id)
        {
            var forma = await _context.Formas
                .Include(f => f.Maquinas)
                .Include(f => f.Produto)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (forma == null) throw new NotFoundException("ID da forma não encontrado.");
            return forma;
        }

        public async Task AdicionarAsync(Forma forma)
        {
            await _context.Formas.AddAsync(forma);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Forma forma)
        {
            _context.Formas.Update(forma);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> ListarNomes()
        {
            return await _context.Formas
                .Select(f => f.Nome)
                .ToListAsync();
        }
    }
}