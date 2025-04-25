using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Services
{
    public class CustoService : ICustoService
    {
        private readonly IProcessoProducaoService _processoProducaoService;
        private readonly IProdutoService _produtoService;

        public CustoService(IProcessoProducaoService processoProducaoService, IProdutoService produtoService)
        {
            _processoProducaoService = processoProducaoService;
            _produtoService = produtoService;
        }

        public async Task<ProducaoPorProdutoEPeriodoResponse> CalcularMediaDeCustoPorPeriodo(ProducaoPorProdutoEPeriodoRequest request)
        {
            await ValidarRequest(request);
            var producoes = await _processoProducaoService.ListarProducoesPorProdutoEPeriodo(
                request.ProdutoId,
                request.DataInicio,
                request.DataFim
            );

            var producoesResponse = await _processoProducaoService.EntityListToResponseList(producoes);

            var custoMedio = producoes
                .Select(p => p.CustoUnitario)
                .Average();

            return new ProducaoPorProdutoEPeriodoResponse(
                request.ProdutoId,
                request.DataInicio,
                DateTime.Now,
                custoMedio,
                producoesResponse
            );
        }

        private async Task ValidarRequest(ProducaoPorProdutoEPeriodoRequest request)
        {
            await _produtoService.BuscarProdutoPorIdAsync(request.ProdutoId);
        }
    }
}
