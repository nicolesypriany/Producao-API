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
    public class FormaController : Controller
    {
        private readonly IFormaService _formaServices;

        public FormaController(IFormaService formaServices)
        {
            _formaServices = formaServices;
        }

        ///<summary>
        ///Obter formas
        ///</summary>
        ///<response code="200">Sucesso</response>
        ///<response code="404">Nenhuma forma encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormaResponse>>> ListarFormas()
        {
            var formas = await _formaServices.ListarFormasAtivas();
            return Ok(_formaServices.EntityListToResponseList(formas));
        }

        ///<summary>
        ///Obter forma por ID
        ///</summary>
        ///<response code="200">Sucesso</response>
        ///<response code="404">Nenhuma forma encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<FormaResponse>> BuscarFormaPorId(int id)
        {
            var forma = await _formaServices.BuscarFormaPorIdAsync(id);
            return Ok(_formaServices.EntityToResponse(forma));
        }

        ///<summary>
        ///Criar uma nova forma
        ///</summary>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="500">Erro de servidor</response>
        [HttpPost]
        public async Task<ActionResult<FormaResponse>> CadastrarForma(FormaRequest request)
        {
            var forma = await _formaServices.AdicionarAsync(request);
            return Ok(_formaServices.EntityToResponse(forma));
        }

        /// <summary>
        /// Atualizar uma forma
        /// </summary>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="404">Nenhuma forma encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<FormaResponse>> AtualizarForma(int id, FormaRequest request)
        {
            var forma = await _formaServices.AtualizarAsync(id, request);
            return Ok(_formaServices.EntityToResponse(forma));
        }

        /// <summary>
        /// Inativar uma forma
        /// </summary>
        ///<response code="200">Sucesso</response>
        ///<response code="404">Nenhuma forma encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<FormaResponse>> InativarForma(int id)
        {
            var forma = await _formaServices.InativarForma(id);
            return Ok(_formaServices.EntityToResponse(forma));
        }
    }
}