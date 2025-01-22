using ProducaoAPI.Models;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IProducaoMateriaPrimaRepository
    {
        Task AdicionarAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima);
    }
}
