 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services;

namespace ProducaoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FormaController : Controller
    {
        private readonly ProducaoContext _context;
        private readonly IFormaRepository _formaRepository;
        public FormaController(IFormaRepository formaRepository, ProducaoContext context)
        {
            _formaRepository = formaRepository;
            _context = context;
        }

        /// <summary>
        /// Obter formas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormaResponse>>> ListarFormas()
        {
            var formas = _formaRepository.ListarFormas();
            if (formas == null) return NotFound();
            return Ok(FormaServices.EntityListToResponseList(formas));
        }

        /// <summary>
        /// Obter forma por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<FormaResponse>> BuscarFormaPorId(int id)
        {
            var forma = _formaRepository.BuscarFormaPorId(id);
            if (forma == null) return NotFound();
            return Ok(FormaServices.EntityToResponse(forma));
        }

        /// <summary>
        /// Criar uma nova forma
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<FormaResponse>> CadastrarForma(FormaRequest req)
        {
            var forma = new Forma(req.Nome, req.ProdutoId, req.PecasPorCiclo);
            await _formaRepository.Adicionar(forma);
            return Ok(forma);
        }

        /// <summary>
        /// Atualizar uma forma
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<FormaResponse>> AtualizarForma(int id, FormaRequest req)
        {
            var forma = _formaRepository.BuscarFormaPorId(id);
            if (forma == null) return NotFound();

            var maquinas = FormaServices.FormaMaquinaRequestToEntity(req.Maquinas, _context);

            forma.Nome = req.Nome;
            forma.ProdutoId = req.ProdutoId;
            forma.PecasPorCiclo = req.PecasPorCiclo;
            forma.Maquinas = maquinas;

            await _formaRepository.Atualizar(forma);
            return Ok(forma);
        }

        /// <summary>
        /// Inativar uma forma
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<FormaResponse>> InativarForma(int id)
        {
            var forma = _formaRepository.BuscarFormaPorId(id);
            if (forma == null) return NotFound();
            forma.Ativo = false;

            await _formaRepository.Atualizar(forma);
            return Ok(forma);
        }
    }
}