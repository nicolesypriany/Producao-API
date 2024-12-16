using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services;

namespace ProducaoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProcessoProducaoController : Controller
    {
        private readonly ProducaoContext _context;
        public ProcessoProducaoController(ProducaoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obter produções
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProcessoProducaoResponse>>> ListarProducoes()
        {
            var producoes = await _context.Producoes
                .Include(p => p.ProducaoMateriasPrimas)
                .ThenInclude(p => p.MateriaPrima)
                .Where(m => m.Ativo == true)
                .ToListAsync();

            if (producoes == null) return NotFound();
            return Ok(ProcessoProducaoServices.EntityListToResponseList(producoes));
        }

        /// <summary>
        /// Obter produção por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProcessoProducaoResponse>> BuscarProducaoPorId(int id)
        {
            var producao = await _context.Producoes
                .Include(p => p.ProducaoMateriasPrimas)
                .ThenInclude(p => p.MateriaPrima)
                .Where(m => m.Ativo == true)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producao == null) return NotFound();
            return Ok(ProcessoProducaoServices.EntityToResponse(producao));
        }

        /// <summary>
        /// Criar uma nova produção
        /// </summary>
        /// <response code="200">Produto cadastrado com sucesso</response>
        /// <response code="400">Request incorreto</response>
        [HttpPost]
        public async Task<ActionResult<ProcessoProducaoResponse>> CadastrarProducao(ProcessoProducaoRequest req)
        {
            var forma = await _context.Formas.FirstOrDefaultAsync(f => f.Id == req.FormaId);
            var producao = new ProcessoProducao(req.Data, req.MaquinaId, req.FormaId, forma.ProdutoId, req.Ciclos);
            await _context.Producoes.AddAsync(producao);
            await _context.SaveChangesAsync();

            var producaoMateriasPrimas = ProcessoProducaoServices.CriarProducoesMateriasPrimas(_context, req.MateriasPrimas, producao.Id);
            foreach (var producaMateriaPrima in producaoMateriasPrimas)
            {
                await _context.ProducoesMateriasPrimas.AddAsync(producaMateriaPrima);
                await _context.SaveChangesAsync();
            }

            return Ok(producao);
        }

        /// <summary>
        /// Atualizar uma produção
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProcessoProducaoResponse>> AtualizarProducao(int id, ProcessoProducaoRequest req)
        {
            var producao = await _context.Producoes.FindAsync(id);
            if (producao == null) return NotFound();

            ProducaoMateriaPrimaServices.VerificarProducoesMateriasPrimasExistentes(_context, id, req.MateriasPrimas);

            producao.Data = req.Data;
            producao.MaquinaId = req.MaquinaId;
            producao.FormaId = req.FormaId;
            producao.Ciclos = req.Ciclos;
            
            await _context.SaveChangesAsync();
            return Ok(producao);
        }

        /// <summary>
        /// Inativar uma produção
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProcessoProducaoResponse>> InativarProducao(int id)
        {
            var producao = await _context.Producoes.FindAsync(id);
            if (producao == null) return NotFound();
            producao.Ativo = false;

            await _context.SaveChangesAsync();
            return Ok(producao);
        }

        /// <summary>
        /// Calcular uma produção
        /// </summary>
        [HttpPost("CalcularProducao/{id}")]
        public async Task<ActionResult<ProcessoProducao>> CalcularProducao(int id)
        {
            ProcessoProducaoServices.CalcularProducao(_context, id);
            return Ok();
        }
    }
}
