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
        private readonly IProdutoRepository _produtoRepository;
        public FormaRepository(ProducaoContext context, IProdutoRepository produtoRepository)
        {
            _context = context;
            _produtoRepository = produtoRepository;
        }

        public async Task<IEnumerable<Forma>> ListarFormasAtivas()
        {
            try
            {
                var formas = await _context.Formas
                    .Where(m => m.Ativo == true)
                    .Include(f => f.Maquinas)
                    .Include(f => f.Produto)
                    .ToListAsync();

                if (formas == null || formas.Count == 0) throw new NotFoundException("Nenhuma forma ativa encontrada.");
                return formas;
            }
            catch (NotFoundException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Forma>> ListarTodasFormas()
        {
            try
            {
                var formas = await _context.Formas
                    .Include(f => f.Maquinas)
                    .Include(f => f.Produto)
                    .ToListAsync();

                if (formas == null || formas.Count == 0) throw new NotFoundException("Nenhuma forma encontrada.");
                return formas;
            }
            catch (NotFoundException)
            {
                throw;
            }
        }

        public async Task<Forma> BuscarFormaPorIdAsync(int id)
        {
            try
            {
                var forma = await _context.Formas
                    .Include(f => f.Maquinas)
                    .Include(f => f.Produto)
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (forma == null) throw new NotFoundException("ID da forma não encontrado.");
                return forma;
            }
            catch (NotFoundException)
            {
                throw;
            }
        }

        public async Task AdicionarAsync(Forma forma)
        {
            try
            {
                await _context.Formas.AddAsync(forma);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AtualizarAsync(Forma forma)
        {
            try
            {
                _context.Formas.Update(forma);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}