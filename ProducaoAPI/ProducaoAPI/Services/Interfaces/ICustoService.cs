using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Services.Interfaces
{
    public interface ICustoService
    {
        Task<ProducaoPorProdutoEPeriodoResponse> CalcularMediaDeCustoPorPeriodo(ProducaoPorProdutoEPeriodoRequest request);
        Task<CustoMensalResponse> CalcularCustoTotalDoMes(CustoPorMesRequest request);
    }
}
