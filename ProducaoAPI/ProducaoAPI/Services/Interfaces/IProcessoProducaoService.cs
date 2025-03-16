using Microsoft.AspNetCore.Mvc;
using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Services.Interfaces
{
    public interface IProcessoProducaoService
    {
        Task<IEnumerable<ProcessoProducao>> ListarProducoesAtivas();
        Task<IEnumerable<ProcessoProducao>> ListarTodasProducoes();
        Task<ProcessoProducao> BuscarProducaoPorIdAsync(int id);
        Task<ProcessoProducao> AdicionarAsync(ProcessoProducaoRequest request);
        Task<ProcessoProducao> AtualizarAsync(int id, ProcessoProducaoRequest request);
        Task<ProcessoProducao> InativarProducao(int id);
        ProcessoProducaoResponse EntityToResponse(ProcessoProducao producao);
        ICollection<ProcessoProducaoResponse> EntityListToResponseList(IEnumerable<ProcessoProducao> producoes);
        Task<List<ProcessoProducaoMateriaPrima>> CriarProducoesMateriasPrimas(ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimas, int ProducaoId);
        Task CalcularProducao(int producaoId);
        Task<Forma> BuscarFormaPorIdAsync(int id);
        Task<FileStreamResult> GerarRelatorioTXT();
        Task<FileStreamResult> GerarRelatorioXLSX();
    }
}
