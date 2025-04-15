using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Services.Interfaces
{
    public interface IProducaoMateriaPrimaService
    {
        ProducaoMateriaPrimaResponse EntityToResponse(ProcessoProducaoMateriaPrima producaoMateriaPrima);
        Task<ICollection<ProducaoMateriaPrimaResponse>> EntityListToResponseList(ICollection<ProcessoProducaoMateriaPrima> producoesMateriasPrimas);
        void VerificarProducoesMateriasPrimasExistentes(int producaoId, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest);
        void CriarOuAtualizarProducaoMateriaPrima(int producaoId, List<int> listaIdNovasMaterias, List<int> listaIdMateriasAtuais, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest);
        Task ExcluirProducaoMateriaPrima(int producaoId, List<int> listaIdNovasMaterias, List<int> listaIdMateriasAtuais);
        decimal RetornarQuantidadeMateriaPrima(int idMateriaPrima, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest);
        Task CompararQuantidadesMateriasPrimas(int producaoId, int materiaPrimaId, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest);
        Task AdicionarAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima);
    }
}
