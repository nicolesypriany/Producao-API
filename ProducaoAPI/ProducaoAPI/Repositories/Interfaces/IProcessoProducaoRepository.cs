using ProducaoAPI.Models;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IProcessoProducaoRepository
    {
        Task<IEnumerable<ProcessoProducao>> ListarProducoesAsync();
        Task<ProcessoProducao> BuscarProducaoPorIdAsync(int id);
        Task AdicionarAsync(ProcessoProducao producao);
        Task AtualizarAsync(ProcessoProducao producao);
    }
}
