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
    public class FreteController : Controller
    {
        private readonly IFreteService _freteServices;
        public FreteController(IFreteService freteService)
        {
            _freteServices = freteService;
        }

        ///<summary>
        ///Calcular um frete
        ///</summary>
        /// <param name="request">Objeto com os dados do frete á ser calculado. Endereço de origem e destino devem estar completos, incluindo rua, número, bairro, cidade e estado.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="500">Erro de servidor</response>
        [HttpPost("Calcular")]
        public async Task<ActionResult<FreteResponse>> CalcularFrete(FreteRequest request)
        {
            var frete = await _freteServices.CalcularPreco(request);
            return Ok(frete);
        }
    }
}
