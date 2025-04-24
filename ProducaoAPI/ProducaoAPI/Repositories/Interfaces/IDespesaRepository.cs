using ProducaoAPI.Models;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IDespesaRepository
    {
        Task<IEnumerable<Despesa>> ListarDespesasAtivas();
        Task<Despesa> BuscarDespesaPorIdAsync(int id);
        Task AdicionarAsync(Despesa despesa);
        Task AtualizarAsync(Despesa despesa);
        Task<IEnumerable<string>> ListarNomes();
    }
}
