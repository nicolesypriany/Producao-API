using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class MateriaPrimaRepository : IMateriaPrimaRepository
    {
        private readonly ProducaoContext _context;
        public MateriaPrimaRepository(ProducaoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MateriaPrima>> ListarMateriasPrimasAtivas()
        {
            var materiasPrimas = await _context.MateriasPrimas
                .Where(m => m.Ativo == true)
                .ToListAsync();

            if (materiasPrimas == null || materiasPrimas.Count == 0) throw new NotFoundException("Nenhuma matéria-prima ativa encontrada.");
            return materiasPrimas;
        }

        public async Task<IEnumerable<MateriaPrima>> ListarTodasMateriasPrimas()
        {
            var materiasPrimas = await _context.MateriasPrimas
                .ToListAsync();

            if (materiasPrimas == null || materiasPrimas.Count == 0) throw new NotFoundException("Nenhuma matéria-prima ativa encontrada.");
            return materiasPrimas;
        }

        public async Task<MateriaPrima> BuscarMateriaPrimaPorIdAsync(int id)
        {
            var materiaPrima = await _context.MateriasPrimas
                .FirstOrDefaultAsync(m => m.Id == id);

            if (materiaPrima == null) throw new NotFoundException("ID da matéria-prima não encontrado.");
            return materiaPrima;
        }

        public async Task AdicionarAsync(MateriaPrima materiaPrima)
        {
            await _context.MateriasPrimas.AddAsync(materiaPrima);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(MateriaPrima materiaPrima)
        {
            _context.MateriasPrimas.Update(materiaPrima);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> ListarNomes()
        {
            return await _context.MateriasPrimas
                .Select(m => m.Nome.ToUpper())
                .ToListAsync();
        }
    }
}
