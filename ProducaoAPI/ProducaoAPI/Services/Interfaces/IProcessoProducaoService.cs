using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Services.Interfaces
{
    public interface IProcessoProducaoService
    {
        Task<IEnumerable<ProcessoProducao>> ListarProducoesAsync();
        Task<ProcessoProducao> BuscarProducaoPorIdAsync(int id);
        Task AdicionarAsync(ProcessoProducao producao);
        Task AtualizarAsync(ProcessoProducao producao);
        ProcessoProducaoResponse EntityToResponse(ProcessoProducao producao);
        ICollection<ProcessoProducaoResponse> EntityListToResponseList(IEnumerable<ProcessoProducao> producoes);
        List<ProcessoProducaoMateriaPrima> CriarProducoesMateriasPrimas(ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimas, int ProducaoId);
        Task CalcularProducao(int producaoId);
        Task<Forma> BuscarFormaPorIdAsync(int id);
    }
}
