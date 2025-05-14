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
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma forma encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormaResponse>>> ListarFormas()
        {
            var formas = await _formaServices.ListarFormasAtivas();
            return Ok(await _formaServices.EntityListToResponseList(formas));
        }

        ///<summary>
        ///Obter forma por ID
        ///</summary>
        ///<param name="id">ID da forma buscada.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma forma encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<FormaResponse>> BuscarFormaPorId(int id)
        {
            var forma = await _formaServices.BuscarFormaPorIdAsync(id);
            return Ok(await _formaServices.EntityToResponse(forma));
        }

        ///<summary>
        ///Criar uma nova forma
        ///</summary>
        ///<param name="request">Objeto com os dados da forma a ser criada.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="500">Erro de servidor</response>
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPost]
        public async Task<ActionResult> CadastrarForma(FormaRequest request)
        {
            await _formaServices.AdicionarAsync(request);
            return Ok();
        }

        /// <summary>
        /// Atualizar uma forma
        /// </summary>
        ///<param name="id">ID da forma a ser atualizada.</param>
        ///<param name="request">Objeto com os dados atualizados da forma.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma forma encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPut("{id}")]
        public async Task<ActionResult> AtualizarForma(int id, FormaRequest request)
        {
            await _formaServices.AtualizarAsync(id, request);
            return Ok();
        }

        /// <summary>
        /// Inativar uma forma
        /// </summary>
        ///<param name="id">ID da forma a ser inativada.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma forma encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<FormaResponse>> InativarForma(int id)
        {
            var forma = await _formaServices.InativarForma(id);
            return Ok(_formaServices.EntityToResponse(forma));
        }
    }
}