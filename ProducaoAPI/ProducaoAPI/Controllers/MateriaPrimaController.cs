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
    public class MateriaPrimaController : Controller
    {
        private readonly ProducaoContext _context;
        public MateriaPrimaController(ProducaoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obter matérias-primas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MateriaPrimaResponse>>> ListarMateriasPrimas()
        {
            var materiasPrimas = await _context.MateriasPrimas.Where(m => m.Ativo == true).ToListAsync();
            if (materiasPrimas == null) return NotFound();
            return Ok(MateriaPrimaServices.EntityListToResponseList(materiasPrimas));
        }

        /// <summary>
        /// Obter matéria-prima por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MateriaPrimaResponse>> BuscarMateriaPrimaPorId(int id)
        {
            var materiaPrima = await _context.MateriasPrimas.FindAsync(id);
            if (materiaPrima == null) return NotFound();
            return Ok(MateriaPrimaServices.EntityToResponse(materiaPrima));
        }

        /// <summary>
        /// Criar uma nova matéria-prima
        /// </summary>
        /// <response code="200">Produto cadastrado com sucesso</response>
        /// <response code="400">Request incorreto</response>
        [HttpPost]
        public async Task<ActionResult<MateriaPrimaResponse>> CadastrarMateriaPrima(MateriaPrimaRequest req)
        {
            var materiaPrima = new MateriaPrima(req.Nome, req.Fornecedor, req.Unidade, req.Preco);
            await _context.MateriasPrimas.AddAsync(materiaPrima);
            await _context.SaveChangesAsync();
            return Ok(MateriaPrimaServices.EntityToResponse(materiaPrima));
        }

        /// <summary>
        /// Atualizar uma matéria-prima
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<MateriaPrimaResponse>> AtualizarMateriaPrima(int id, MateriaPrimaRequest req)
        {
            var materiaPrima = await _context.MateriasPrimas.FindAsync(id);
            if (materiaPrima == null) return NotFound();

            materiaPrima.Nome = req.Nome;
            materiaPrima.Fornecedor = req.Fornecedor;
            materiaPrima.Unidade = req.Unidade;
            materiaPrima.Preco = req.Preco;

            await _context.SaveChangesAsync();
            return Ok(MateriaPrimaServices.EntityToResponse(materiaPrima));
        }

        /// <summary>
        /// Inativar uma matéria-prima
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<MateriaPrimaResponse>> InativarProduto(int id)
        {
            var materiaPrima = await _context.MateriasPrimas.FindAsync(id);
            if (materiaPrima == null) return NotFound();
            materiaPrima.Ativo = false;

            await _context.SaveChangesAsync();
            return Ok(MateriaPrimaServices.EntityToResponse(materiaPrima));
        }

        /// <summary>
        /// Cadastrar uma matéria-prima por importação do XML de uma nota fiscal
        /// </summary>
        [HttpPost("ImportarXML")]
        public async Task<ActionResult<MateriaPrimaResponse>> CadastrarMateriaPrimaPorXML(IFormFile arquivoXML)
        {
            var novaMateriaPrima = MateriaPrimaServices.CriarMateriaPrimaPorXML(_context, arquivoXML);
            var materiaPrima = await _context.MateriasPrimas.FindAsync(novaMateriaPrima.Id);
            return Ok(MateriaPrimaServices.EntityToResponse(materiaPrima));
        }
    }
}
