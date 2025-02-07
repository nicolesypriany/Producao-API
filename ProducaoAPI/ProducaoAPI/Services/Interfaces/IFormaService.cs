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
        Task AdicionarAsync(Forma forma);
        Task AtualizarAsync(Forma forma);
        FormaResponse EntityToResponse(Forma forma);
        ICollection<FormaResponse> EntityListToResponseList(IEnumerable<Forma> forma);
        Task<List<Maquina>> FormaMaquinaRequestToEntity(ICollection<FormaMaquinaRequest> maquinas);
        Task ValidarDados(FormaRequest request);
    }
}
