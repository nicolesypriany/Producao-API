using ProducaoAPI.Models;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IFormaRepository : IBaseRepository<Forma>
    {
        IEnumerable<Forma> ListarFormas();
        Forma BuscarFormaPorId(int id);
    }
}
