using ProducaoAPI.Models;
using ProducaoAPI.Requests;

namespace ProducaoAPI.Services.Interfaces
{
    public interface IDespesaService
    {
        Task<IEnumerable<Despesa>> ListarDespesasAtivas();
        Task<Despesa> BuscarDespesaPorIdAsync(int id);
        Task<Despesa> AdicionarAsync(DespesaRequest request);
        Task<Despesa> AtualizarAsync(int id, DespesaRequest request);
        Task<Despesa> InativarDespesa(int id);
    }
}
