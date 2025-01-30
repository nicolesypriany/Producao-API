using ProducaoAPI.Models;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IMaquinaRepository
    {
        Task<IEnumerable<Maquina>> ListarMaquinasAsync();
        Task<Maquina> BuscarMaquinaPorIdAsync(int id);
        Task AdicionarAsync(Maquina maquina);
        Task AtualizarAsync(Maquina maquina);
    }
}
