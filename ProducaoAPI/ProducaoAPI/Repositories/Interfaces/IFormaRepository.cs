using ProducaoAPI.Models;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IFormaRepository
    {
        Task<IEnumerable<Forma>> ListarFormasAsync();
        Task<Forma> BuscarFormaPorIdAsync(int id);
        Task AdicionarAsync(Forma forma);
        Task AtualizarAsync(Forma forma);
    }
}