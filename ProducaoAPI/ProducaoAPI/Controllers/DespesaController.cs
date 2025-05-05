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
    public class DespesaController : Controller
    {
        private readonly IDespesaService _despesaServices;

        public DespesaController(IDespesaService despesaServices)
        {
            _despesaServices = despesaServices;
        }

        ///<summary>
        ///Obter despesas
        ///</summary>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma despesa encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DespesaResponse>>> ListarDespesas()
        {
            var despesas = await _despesaServices.ListarDespesasAtivas();
            return Ok(_despesaServices.EntityListToResponseList(despesas));
        }

        ///<summary>
        ///Obter despesa por ID
        ///</summary>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma despesa encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<DespesaResponse>> BuscarDespesaPorId(int id)
        {
            var despesa = await _despesaServices.BuscarDespesaPorIdAsync(id);
            return Ok(_despesaServices.EntityToResponse(despesa));
        }

        ///<summary>
        ///Criar uma nova despesa
        ///</summary>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="500">Erro de servidor</response>
        [Authorize(Roles = "Administrador")]
        [Authorize(Roles = "Gerente")]
        [HttpPost]
        public async Task<ActionResult> CadastrarDespesa(DespesaRequest request)
        {
            await _despesaServices.AdicionarAsync(request);
            return Ok();
        }

        /// <summary>
        /// Atualizar uma despesa
        /// </summary>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma despesa encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [Authorize(Roles = "Administrador")]
        [Authorize(Roles = "Gerente")]
        [HttpPut("{id}")]
        public async Task<ActionResult<DespesaResponse>> AtualizarDespesa(int id, DespesaRequest request)
        {
            var despesa = await _despesaServices.AtualizarAsync(id, request);
            return Ok(_despesaServices.EntityToResponse(despesa));
        }

        /// <summary>
        /// Inativar uma despesa
        /// </summary>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma despesa encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [Authorize(Roles = "Administrador")]
        [Authorize(Roles = "Gerente")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<DespesaResponse>> InativarDespesa(int id)
        {
            var despesa = await _despesaServices.InativarDespesa(id);
            return Ok(_despesaServices.EntityToResponse(despesa));
        }
    }
}
