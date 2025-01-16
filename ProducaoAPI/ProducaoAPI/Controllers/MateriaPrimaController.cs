using Microsoft.AspNetCore.Mvc;
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
    //[Authorize]
    public class MateriaPrimaController : Controller
    {
        private readonly ProducaoContext _context;
        private readonly IMateriaPrimaRepository _materiaPrimaRepository;
        public MateriaPrimaController(IMateriaPrimaRepository materiaPrimaRepository, ProducaoContext context)
        {
            _materiaPrimaRepository = materiaPrimaRepository;
            _context = context;
        }

        /// <summary>
        /// Obter matérias-primas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MateriaPrimaResponse>>> ListarMateriasPrimas()
        {
            var materiasPrimas = _materiaPrimaRepository.ListarMateriasPrimas();
            if (materiasPrimas == null) return NotFound();
            return Ok(MateriaPrimaServices.EntityListToResponseList(materiasPrimas));
        }

        /// <summary>
        /// Obter matéria-prima por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MateriaPrimaResponse>> BuscarMateriaPrimaPorId(int id)
        {
            var materiaPrima = _materiaPrimaRepository.BuscarPorID(id);
            if (materiaPrima == null) return NotFound();
            return Ok(MateriaPrimaServices.EntityToResponse(materiaPrima));
        }

        /// <summary>
        /// Criar uma nova matéria-prima
        /// </summary>
        /// <response code="200">Matéria-prima cadastrada com sucesso</response>
        /// <response code="400">Request incorreto</response>
        [HttpPost]
        public async Task<ActionResult<MateriaPrimaResponse>> CadastrarMateriaPrima(MateriaPrimaRequest req)
        {
            var materiaPrima = new MateriaPrima(req.Nome, req.Fornecedor, req.Unidade, req.Preco);
            await _materiaPrimaRepository.Adicionar(materiaPrima);
            return Ok(MateriaPrimaServices.EntityToResponse(materiaPrima));
        }

        /// <summary>
        /// Atualizar uma matéria-prima
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<MateriaPrimaResponse>> AtualizarMateriaPrima(int id, MateriaPrimaRequest req)
        {
            var materiaPrima = _materiaPrimaRepository.BuscarPorID(id);
            if (materiaPrima == null) return NotFound();

            materiaPrima.Nome = req.Nome;
            materiaPrima.Fornecedor = req.Fornecedor;
            materiaPrima.Unidade = req.Unidade;
            materiaPrima.Preco = req.Preco;


            await _materiaPrimaRepository.Atualizar(materiaPrima);
            return Ok(MateriaPrimaServices.EntityToResponse(materiaPrima));
        }

        /// <summary>
        /// Inativar uma matéria-prima
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<MateriaPrimaResponse>> InativarProduto(int id)
        {
            var materiaPrima = _materiaPrimaRepository.BuscarPorID(id);
            if (materiaPrima == null) return NotFound();
            materiaPrima.Ativo = false;

            await _materiaPrimaRepository.Atualizar(materiaPrima);
            return Ok(MateriaPrimaServices.EntityToResponse(materiaPrima));
        }

        /// <summary>
        /// Cadastrar uma matéria-prima por importação do XML de uma nota fiscal
        /// </summary>
        [HttpPost("ImportarXML")]
        public async Task<ActionResult<MateriaPrimaResponse>> CadastrarMateriaPrimaPorXML(IFormFile arquivoXML)
        {
            var novaMateriaPrima = MateriaPrimaServices.CriarMateriaPrimaPorXML(_context, arquivoXML);
            var materiaPrima = _materiaPrimaRepository.BuscarPorID(novaMateriaPrima.Id);
            return Ok(MateriaPrimaServices.EntityToResponse(materiaPrima));
        }
    }
}
