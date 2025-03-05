using ProducaoAPI.Models;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IFormaRepository
    {
        Task<IEnumerable<Forma>> ListarFormasAtivas();
        Task<IEnumerable<Forma>> ListarTodasFormas();
        Task<Forma> BuscarFormaPorIdAsync(int id);
        Task AdicionarAsync(Forma forma);
        Task AtualizarAsync(Forma forma);
        Task<IEnumerable<string>> ListarNomes();
    }
}