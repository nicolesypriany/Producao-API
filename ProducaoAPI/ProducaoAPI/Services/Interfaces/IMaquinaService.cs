using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Services.Interfaces
{
    public interface IMaquinaService
    {
        Task<IEnumerable<Maquina>> ListarMaquinasAtivas();
        Task<IEnumerable<Maquina>> ListarTodasMaquinas();
        Task<Maquina> BuscarMaquinaPorIdAsync(int id);
        Task<Maquina> AdicionarAsync(MaquinaRequest request);
        Task<Maquina> AtualizarAsync(int id, MaquinaRequest request);
        Task<Maquina> InativarMaquina(int id);
        MaquinaResponse EntityToResponse(Maquina maquina);
        ICollection<MaquinaResponse> EntityListToResponseList(IEnumerable<Maquina> maquinas);
    }
}