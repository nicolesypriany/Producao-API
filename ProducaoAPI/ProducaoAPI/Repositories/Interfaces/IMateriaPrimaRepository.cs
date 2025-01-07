using ProducaoAPI.Models;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IMateriaPrimaRepository : IBaseRepository<MateriaPrima>
    {
        IEnumerable<MateriaPrima> ListarMateriasPrimas();
    }
}
