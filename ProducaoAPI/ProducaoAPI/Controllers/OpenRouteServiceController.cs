using Microsoft.AspNetCore.Mvc;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services;
using System.Text.Json;

namespace ProducaoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OpenRouteServiceController : Controller
    {
        private readonly RouteServices _routeServices;
        public OpenRouteServiceController()
        {
            
        }

        [HttpPost("BuscarCoordenadas")]
        public async Task<IActionResult> BuscarCoordenadas(string endereco)
        {
            try
            {
                var coordenadas = await _routeServices.BuscarCoordenadas(endereco);
                return Ok(coordenadas);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
