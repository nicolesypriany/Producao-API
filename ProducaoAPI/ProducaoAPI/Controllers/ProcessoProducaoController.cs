using Microsoft.AspNetCore.Mvc;
using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProcessoProducaoResponse>>> ListarProducoes()
        {
            try
            {
                var producoes = await _processoProducaoService.ListarProducoesAsync();
                if (producoes == null) return NotFound();
                return Ok(_processoProducaoService.EntityListToResponseList(producoes));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Obter produção por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProcessoProducaoResponse>> BuscarProducaoPorId(int id)
        {
            try
            {
                var producao = await _processoProducaoService.BuscarProducaoPorIdAsync(id);
                if (producao == null) return NotFound();
                return Ok(_processoProducaoService.EntityToResponse(producao));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Criar uma nova produção
        /// </summary>
        /// <response code="200">Produto cadastrado com sucesso</response>
        /// <response code="400">Request incorreto</response>
        [HttpPost]
        public async Task<ActionResult<ProcessoProducaoResponse>> CadastrarProducao(ProcessoProducaoRequest request)
        {
            try
            {
                await _processoProducaoService.ValidarDados(request);
                var forma = await _processoProducaoService.BuscarFormaPorIdAsync(request.FormaId);
                //var forma = await _context.Formas.FirstOrDefaultAsync(f => f.Id == req.FormaId);
                var producao = new ProcessoProducao(request.Data, request.MaquinaId, forma.Id, forma.ProdutoId, request.Ciclos);

                await _processoProducaoService.AdicionarAsync(producao);

                var producaoMateriasPrimas = _processoProducaoService.CriarProducoesMateriasPrimas(request.MateriasPrimas, producao.Id);
                foreach (var producaMateriaPrima in producaoMateriasPrimas)
                {
                    await _producaoMateriaPrimaService.AdicionarAsync(producaMateriaPrima);
                }

                return Ok(producao);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Atualizar uma produção
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProcessoProducaoResponse>> AtualizarProducao(int id, ProcessoProducaoRequest request)
        {
            try
            {
                await _processoProducaoService.ValidarDados(request);
                var producao = await _processoProducaoService.BuscarProducaoPorIdAsync(id);
                if (producao == null) return NotFound();

                _producaoMateriaPrimaService.VerificarProducoesMateriasPrimasExistentes(id, request.MateriasPrimas);

                producao.Data = request.Data;
                producao.MaquinaId = request.MaquinaId;
                producao.FormaId = request.FormaId;
                producao.Ciclos = request.Ciclos;

                await _processoProducaoService.AtualizarAsync(producao);
                return Ok(producao);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message) ;
            }
        }

        /// <summary>
        /// Inativar uma produção
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProcessoProducaoResponse>> InativarProducao(int id)
        {
            try
            {
                var producao = await _processoProducaoService.BuscarProducaoPorIdAsync(id);
                if (producao == null) return NotFound();
                producao.Ativo = false;

                await _processoProducaoService.AtualizarAsync(producao);
                return Ok(producao);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Calcular uma produção
        /// </summary>
        [HttpPost("CalcularProducao/{id}")]
        public async Task<ActionResult<ProcessoProducao>> CalcularProducao(int id)
        {
            try
            {
                await _processoProducaoService.CalcularProducao(id);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
