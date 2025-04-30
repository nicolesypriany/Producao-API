using ProducaoAPI.Models;
using ProducaoAPI.Requests;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface ILogRepository
    {
        Task AdicionarAsync(Log log);
        Task<IEnumerable<Log>> BuscarLogs(LogRequest request);
    }
}
