using ProducaoAPI.Models;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IMaquinaRepository
    {
        Task<IEnumerable<Maquina>> ListarMaquinasAtivas();
        Task<IEnumerable<Maquina>> ListarTodasMaquinas();
        Task<Maquina> BuscarMaquinaPorIdAsync(int id);
        Task AdicionarAsync(Maquina maquina);
        Task AtualizarAsync(Maquina maquina);
        Task<IEnumerable<string>> ListarNomes();
    }
}
