using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
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

        public async Task<IEnumerable<ProcessoProducao>> ListarProducoesAtivas()
        {
            try
            {
                var producoes = await _context.Producoes
               .Include(p => p.ProducaoMateriasPrimas)
               .ThenInclude(p => p.MateriaPrima)
               .Where(m => m.Ativo == true)
               .ToListAsync();

                if (producoes == null || producoes.Count == 0) throw new NotFoundException("Nenhuma produção encontrada.");
                return producoes;
            }
            catch (NotFoundException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProcessoProducao>> ListarTodasProducoes()
        {
            try
            {
                var producoes = await _context.Producoes
               .Include(p => p.ProducaoMateriasPrimas)
               .ThenInclude(p => p.MateriaPrima)
               .ToListAsync();

                if (producoes == null || producoes.Count == 0) throw new NotFoundException("Nenhuma produção ativa encontrada.");
                return producoes;
            }
            catch (NotFoundException)
            {
                throw;
            }
        }

        public async Task<ProcessoProducao> BuscarProducaoPorIdAsync(int id)
        {
            try
            {
                var producao = await _context.Producoes
               .Include(p => p.ProducaoMateriasPrimas)
               .ThenInclude(p => p.MateriaPrima)
               .Where(m => m.Ativo == true)
               .FirstOrDefaultAsync(p => p.Id == id);

                if (producao == null) throw new NotFoundException("ID da produção não encontrado");
                return producao;
            }
            catch (NotFoundException)
            {
                throw;
            }
        }

        public async Task AdicionarAsync(ProcessoProducao producao)
        {
            try
            {
                await _context.Producoes.AddAsync(producao);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AtualizarAsync(ProcessoProducao producao)
        {
            try
            {
                _context.Producoes.Update(producao);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
