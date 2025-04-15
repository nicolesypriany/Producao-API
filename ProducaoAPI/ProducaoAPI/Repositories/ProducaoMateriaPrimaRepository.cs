using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;

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

        public async Task RemoverAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima)
        {
            _context.ProducoesMateriasPrimas.Remove(producaoMateriaPrima);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProcessoProducaoMateriaPrima>> ListarProcessosProducaoMateriaPrima(int producaoId)
        {
            var producoesMateriaPrima = await _context.ProducoesMateriasPrimas
                .Where(p => p.ProducaoId == producaoId)
                .ToListAsync();

            if (producoesMateriaPrima is null || producoesMateriaPrima.Count == 0) throw new NotFoundException("Nenhuma matéria-prima da produção encontrada.");
            return producoesMateriaPrima;
        }

        public async Task<ProcessoProducaoMateriaPrima> BuscarProcessoProducaoMateriaPrima(int producaoId, int materiaPrimaId)
        {
            var producaoMateriaPrima = _context.ProducoesMateriasPrimas
                .Where(p => p.ProducaoId == producaoId)
                .Where(p => p.MateriaPrimaId == materiaPrimaId)
                .FirstOrDefault();

            if (producaoMateriaPrima is null) throw new NotFoundException("Nenhuma matéria-prima da produção encontrada.");
            return producaoMateriaPrima;
        }

        public async Task AtualizarAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima)
        {
            _context.ProducoesMateriasPrimas.Update(producaoMateriaPrima);
            await _context.SaveChangesAsync();
        }
    }
}
