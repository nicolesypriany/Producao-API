using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;

namespace ProducaoAPI.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly ProducaoContext _context;

        public LogRepository(ProducaoContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Log log)
        {
            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Log>> BuscarLogs(LogRequest request)
        {
            var logs = await _context.Logs
                .Where(l => l.Objeto == request.Objeto)
                .Where(l => l.IdObjeto == request.ObjetoId)
                .OrderBy(l => l.Data)
                .ToListAsync();

            if (logs is null || logs.Count == 0) throw new NotFoundException("Nenhum log encontrado.");
            return logs;
        }
    }
}
