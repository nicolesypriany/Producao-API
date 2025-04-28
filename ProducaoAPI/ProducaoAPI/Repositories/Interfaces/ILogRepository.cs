using ProducaoAPI.Models;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface ILogRepository
    {
        Task AdicionarAsync(Log log);
    }
}
