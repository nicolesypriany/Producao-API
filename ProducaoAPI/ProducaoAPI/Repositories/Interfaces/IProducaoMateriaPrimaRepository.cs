using ProducaoAPI.Models;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IProducaoMateriaPrimaRepository
    {
        Task AdicionarAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima);
        Task RemoverAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima);
        Task<IEnumerable<ProcessoProducaoMateriaPrima>> ListarProcessosProducaoMateriaPrima(int producaoId);
        Task<ProcessoProducaoMateriaPrima> BuscarProcessoProducaoMateriaPrima(int producaoId, int materiaPrimaId);
        Task AtualizarAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima);
    }
}
