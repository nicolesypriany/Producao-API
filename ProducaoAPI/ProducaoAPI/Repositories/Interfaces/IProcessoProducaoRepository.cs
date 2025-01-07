using ProducaoAPI.Models;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IProcessoProducaoRepository : IBaseRepository<ProcessoProducao>
    {
        IEnumerable<ProcessoProducao> ListarProducoes();
        ProcessoProducao BuscarProducaoPorId(int id);
    }
}
