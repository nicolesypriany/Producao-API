using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CustoController : Controller
    {
        private readonly ICustoService _custoService;
        public CustoController(ICustoService custoService)
        {
            _custoService = custoService;
        }

        ///<summary>
        ///Calcular média de custo por produto e por período
        ///</summary>
        /// <param name="request">Objeto com os dados do custo que será calculado: Id do produto, data inicial e data final.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhum resultado encontrado</response>
        ///<response code="500">Erro de servidor</response>
        [HttpPost("CustoMedioPorProdutoEPeriodo")]
        public async Task<ActionResult<ProducaoPorProdutoEPeriodoResponse>> CalcularMediaDeCustoPorPeriodo([FromBody] ProducaoPorProdutoEPeriodoRequest request)
        {
            var response = await _custoService.CalcularMediaDeCustoPorPeriodo(request);
            return Ok(response);
        }

        ///<summary>
        ///Calcular custo mensal
        ///</summary>
        ///<param name="request">Objeto com os dados do custo que será calculado: mês e ano em forma de número. Ex: Janeiro = 1.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhum resultado encontrado</response>
        ///<response code="500">Erro de servidor</response>
        [HttpPost("CustoTotalMensal")]
        public async Task<ActionResult<CustoMensalResponse>> CalcularCustoMensal([FromBody] CustoPorMesRequest request)
        {
            var response = await _custoService.CalcularCustoTotalDoMes(request);
            return Ok(response);
        }
    }
}
