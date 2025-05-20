using ProducaoAPI.Exceptions;
using ProducaoAPI.Extensions;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Services
{
    public class CustoService : ICustoService
    {
        private readonly IProcessoProducaoService _processoProducaoService;
        private readonly IProdutoService _produtoService;
        private readonly IDespesaService _despesaService;

        public CustoService(IProcessoProducaoService processoProducaoService, IProdutoService produtoService, IDespesaService despesaService)
        {
            _processoProducaoService = processoProducaoService;
            _produtoService = produtoService;
            _despesaService = despesaService;
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

        public async Task<CustoMensalResponse> CalcularCustoTotalDoMes(CustoPorMesRequest request)
        {
            DateTime dataInicio = new(request.Ano, request.Mes, 1);
            DateTime dataFim = dataInicio.AddMonths(1).AddDays(-1);

            var producoesDoMes = await _processoProducaoService.ListarProducoesPorMes(dataInicio, dataFim);

            var custoDoMes = producoesDoMes
                .Select(p => p.CustoTotal)
                .Sum();

            var despesas = await _despesaService.ListarDespesasAtivas();
            var totalDespesas = despesas
                .Select(d => d.Valor)
                .Sum();

            var custoTotalMensal = custoDoMes + totalDespesas;

            var despesasResponseList = despesas.MapListToResponse();
            var producoesResponseList = await _processoProducaoService.EntityListToResponseList(producoesDoMes);

            return new CustoMensalResponse(
                custoTotalMensal,
                totalDespesas,
                custoDoMes,
                despesasResponseList,
                producoesResponseList
            );
        }

        private async Task ValidarRequest(ProducaoPorProdutoEPeriodoRequest request)
        {
            await _produtoService.BuscarProdutoPorIdAsync(request.ProdutoId);
            if (request.DataInicio > request.DataFim) throw new BadRequestException("A data final não pode ser anterior à data inicial.");
        }
    }
}
