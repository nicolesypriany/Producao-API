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
    public class MaquinaController : Controller
    {
        private readonly IMaquinaService _maquinaService;

        public MaquinaController(IMaquinaService maquinaService)
        {
            _maquinaService = maquinaService;
        }

        /// <summary>
        /// Obter máquinas
        /// </summary>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma máquina encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaquinaResponse>>> ListarMaquinas()
        {
            var maquinas = await _maquinaService.ListarMaquinasAtivas();
            return Ok(_maquinaService.EntityListToResponseList(maquinas));
        }

        /// <summary>
        /// Obter máquina por ID
        /// </summary>
        ///<param name="id">ID da máquina buscada.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma máquina encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<MaquinaResponse>> BuscarMaquinaPorId(int id)
        {
            var maquina = await _maquinaService.BuscarMaquinaPorIdAsync(id);
            return Ok(_maquinaService.EntityToResponse(maquina));
        }

        /// <summary>
        /// Criar uma nova máquina
        /// </summary>
        ///<param name="request">Objeto com os dados da máquina a ser criada.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="500">Erro de servidor</response>
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPost]
        public async Task<ActionResult<MaquinaResponse>> CadastrarMaquina(MaquinaRequest request)
        {
            var maquina = await _maquinaService.AdicionarAsync(request);
            return Ok(_maquinaService.EntityToResponse(maquina));
        }

        /// <summary>
        /// Atualizar uma máquina
        /// </summary>
        ///<param name="id">ID da máquina a ser atualizada.</param>
        ///<param name="request">Objeto com os dados atualizados da máquina.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma máquina encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPut("{id}")]
        public async Task<ActionResult<MaquinaResponse>> AtualizarMaquina(int id, MaquinaRequest request)
        {
            var maquina = await _maquinaService.AtualizarAsync(id, request);
            return Ok(_maquinaService.EntityToResponse(maquina));
        }

        /// <summary>
        /// Inativar uma máquina
        /// </summary>
        ///<param name="id">ID da máquina a ser inativada.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma máquina encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<MaquinaResponse>> InativarMaquina(int id)
        {
            var maquina = await _maquinaService.InativarMaquina(id);
            return Ok(_maquinaService.EntityToResponse(maquina));
        }
    }
}