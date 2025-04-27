using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class DespesaRepository : IDespesaRepository
    {
        private readonly ProducaoContext _context;
        public DespesaRepository(ProducaoContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Despesa despesa)
        {
            await _context.Despesas.AddAsync(despesa);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Despesa despesa)
        {
            _context.Despesas.Update(despesa);
            await _context.SaveChangesAsync();
        }

        public async Task<Despesa> BuscarDespesaPorIdAsync(int id)
        {
            var despesa = await _context.Despesas
                .FirstOrDefaultAsync(d => d.Id == id);

            if (despesa is null) throw new NotFoundException("ID da despesa não encontrado.");
            return despesa;
        }

        public async Task<IEnumerable<Despesa>> ListarDespesasAtivas()
        {
            var despesas = await _context.Despesas
                .Where(d => d.Ativo)
                .ToListAsync();

            if (despesas is null || despesas.Count == 0) throw new NotFoundException("Nenhuma despesa encontrada.");
            return despesas;
        }

        public async Task<IEnumerable<string>> ListarNomes()
        {
            return await _context.Despesas
                .Select(d => d.Nome.ToUpper())
                .ToListAsync();
        }
    }
}
