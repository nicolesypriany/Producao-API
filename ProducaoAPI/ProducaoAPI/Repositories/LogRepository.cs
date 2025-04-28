using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

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
    }
}
