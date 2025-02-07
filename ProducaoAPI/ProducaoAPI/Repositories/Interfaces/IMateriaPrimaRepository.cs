using ProducaoAPI.Models;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IMateriaPrimaRepository
    {
        Task<IEnumerable<MateriaPrima>> ListarMateriasPrimasAtivas();
        Task<IEnumerable<MateriaPrima>> ListarTodasMateriasPrimas();
        Task<MateriaPrima> BuscarMateriaPorIdAsync(int id);
        Task AdicionarAsync(MateriaPrima materiaPrima);
        Task AtualizarAsync(MateriaPrima materiaPrima);
    }
}
