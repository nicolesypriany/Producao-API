using ProducaoAPI.Models;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IMaquinaRepository : IBaseRepository<Maquina>
    {
        IEnumerable<Maquina> ListarMaquinas();
    }
}
