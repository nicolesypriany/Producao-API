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
    public class LogController : Controller
    {
        private readonly ILogServices _logServices;

        public LogController(ILogServices logServices)
        {
            _logServices = logServices;
        }

        ///<summary>
        ///Obter logs
        ///</summary>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma despesa encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpPost]
        public async Task<IEnumerable<LogResponse>> ListarLogs(LogRequest request)
        {
            var logs = await _logServices.BuscarLogs(request);
            return logs;
        }
    }
}
