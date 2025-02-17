using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Services.Interfaces
{
    public interface IFormaService
    {
        Task<IEnumerable<Forma>> ListarFormasAtivas();
        Task<IEnumerable<Forma>> ListarTodasFormas();
        Task<Forma> BuscarFormaPorIdAsync(int id);
        Task<Forma> AdicionarAsync(FormaRequest request);
        Task AtualizarAsync(Forma forma);
        FormaResponse EntityToResponse(Forma forma);
        ICollection<FormaResponse> EntityListToResponseList(IEnumerable<Forma> forma);
        Task<List<Maquina>> FormaMaquinaRequestToEntity(ICollection<FormaMaquinaRequest> maquinas);
        Task ValidarDadosParaCadastrar(FormaRequest request);
        Task ValidarDadosParaAtualizar(FormaRequest request, int id);
    }
}
