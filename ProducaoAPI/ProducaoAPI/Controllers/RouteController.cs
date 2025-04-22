using Microsoft.AspNetCore.Mvc;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services;

namespace ProducaoAPI.Controllers
{

    [Route("[controller]")]
    [ApiController]
    //  [Authorize]
    public class RouteController : Controller
    {
        private readonly RouteServices _routeService;
        public RouteController()
        {
            _routeService = new RouteServices();
        }

        [HttpPost("coordenadas")]
        public async Task<ActionResult<RouteResponse>> CalcularFrete(FreteRequest request)
        {
            var frete = await _routeService.CalcularPreco(request);
            return Ok(frete);
        }
    }
}
