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

        [HttpPost("Calcular")]
        public async Task<ActionResult<FreteResponse>> CalcularFrete(FreteRequest request)
        {
            var frete = await _freteServices.CalcularPreco(request);
            return Ok(frete);
        }
    }
}
