using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Services.Interfaces
{
    public interface IFreteService
    {
        Task<FreteResponse> CalcularPreco(FreteRequest request);
    }
}
