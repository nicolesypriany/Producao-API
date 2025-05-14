using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ProcessoProducaoController : Controller
    {
        private readonly IProcessoProducaoService _processoProducaoService;
        private readonly IProducaoMateriaPrimaService _producaoMateriaPrimaService;
        public ProcessoProducaoController(IProcessoProducaoService processoProducaoService, IProducaoMateriaPrimaService producaoMateriaPrimaService)
        {
            _processoProducaoService = processoProducaoService;
            _producaoMateriaPrimaService = producaoMateriaPrimaService;
        }

        /// <summary>
        /// Obter produções
        /// </summary>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma produção encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProcessoProducaoResponse>>> ListarProducoes()
        {
            var producoes = await _processoProducaoService.ListarProducoesAtivas();
            return Ok(await _processoProducaoService.EntityListToResponseList(producoes));
        }

        /// <summary>
        /// Obter produção por ID
        /// </summary>
        ///<param name="id">ID da produção buscada.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma produção encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProcessoProducaoResponse>> BuscarProducaoPorId(int id)
        {
            var producao = await _processoProducaoService.BuscarProducaoPorIdAsync(id);
            return Ok(await _processoProducaoService.EntityToResponse(producao));
        }

        /// <summary>
        /// Criar uma nova produção
        /// </summary>
        ///<param name="request">Objeto com os dados da produção a ser criada.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="500">Erro de servidor</response>
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPost]
        public async Task<ActionResult<ProcessoProducaoResponse>> CadastrarProducao(ProcessoProducaoRequest request)
        {
            var producao = await _processoProducaoService.AdicionarAsync(request);
            return Ok(await _processoProducaoService.EntityToResponse(producao));
        }

        /// <summary>
        /// Atualizar uma produção
        /// </summary>
        ///<param name="id">ID da matéria-prima a ser atualizada.</param>
        ///<param name="request">Objeto com os dados atualizados da matéria-prima.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma produção encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ProcessoProducaoResponse>> AtualizarProducao(int id, ProcessoProducaoRequest request)
        {
            var producao = await _processoProducaoService.AtualizarAsync(id, request);
            return Ok(await _processoProducaoService.EntityToResponse(producao));
        }

        /// <summary>
        /// Inativar uma produção
        /// </summary>
        ///<param name="id">ID da produção a ser inativada.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma produção encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProcessoProducaoResponse>> InativarProducao(int id)
        {
            var producao = await _processoProducaoService.InativarProducao(id);
            return Ok(await _processoProducaoService.EntityToResponse(producao));
        }

        /// <summary>
        /// Calcular uma produção
        /// </summary>
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPost("CalcularProducao/{id}")]
        public async Task<ActionResult<ProcessoProducao>> CalcularProducao(int id)
        {
            await _processoProducaoService.CalcularProducao(id);
            var producao = await _processoProducaoService.BuscarProducaoPorIdAsync(id);
            return Ok(await _processoProducaoService.EntityToResponse(producao));
        }

        /// <summary>
        /// Calcular todas as produções
        /// </summary>
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpGet("CalcularProducoes")]
        public async Task<ActionResult> CalcularProducoes()
        {
            var producoes = await _processoProducaoService.ListarProducoesAtivas();
            foreach(var producao in producoes)
            {
                await _processoProducaoService.CalcularProducao(producao.Id);
            }
            return Ok();
        }

        /// <summary>
        /// Gerar relatório das Produções em arquivo TXT
        /// </summary>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet("GerarRelatórioTXT")]
        public async Task<IActionResult> GerarRelatorioTXT()
        {
            return await _processoProducaoService.GerarRelatorioTXT();
        }

        /// <summary>
        /// Gerar relatório das Produções em arquivo Excel
        /// </summary>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet("GerarRelatórioXLSX")]
        public async Task<IActionResult> GerarRelatorioXLSX()
        {
            return await _processoProducaoService.GerarRelatorioXLSX();
        }
    }
}
