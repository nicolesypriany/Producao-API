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
    public class MateriaPrimaController : Controller
    {
        private readonly IMateriaPrimaService _materiaPrimaService;
        public MateriaPrimaController(IMateriaPrimaService materiaPrimaService)
        {
            _materiaPrimaService = materiaPrimaService;
        }

        /// <summary>
        /// Obter matérias-primas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MateriaPrimaResponse>>> ListarMateriasPrimas()
        {
            var materiasPrimas = await _materiaPrimaService.ListarMateriasAsync();
            if (materiasPrimas == null) return NotFound();
            return Ok(_materiaPrimaService.EntityListToResponseList(materiasPrimas));
        }

        /// <summary>
        /// Obter matéria-prima por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MateriaPrimaResponse>> BuscarMateriaPrimaPorId(int id)
        {
            var materiaPrima = await _materiaPrimaService.BuscarMateriaPorIdAsync(id);
            if (materiaPrima == null) return NotFound();
            return Ok(_materiaPrimaService.EntityToResponse(materiaPrima));
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
            await _materiaPrimaService.AdicionarAsync(materiaPrima);
            return Ok(_materiaPrimaService.EntityToResponse(materiaPrima));
        }

        /// <summary>
        /// Atualizar uma matéria-prima
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<MateriaPrimaResponse>> AtualizarMateriaPrima(int id, MateriaPrimaRequest req)
        {
            var materiaPrima = await _materiaPrimaService.BuscarMateriaPorIdAsync(id);
            if (materiaPrima == null) return NotFound();

            materiaPrima.Nome = req.Nome;
            materiaPrima.Fornecedor = req.Fornecedor;
            materiaPrima.Unidade = req.Unidade;
            materiaPrima.Preco = req.Preco;


            await _materiaPrimaService.AtualizarAsync(materiaPrima);
            return Ok(_materiaPrimaService.EntityToResponse(materiaPrima));
        }

        /// <summary>
        /// Inativar uma matéria-prima
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<MateriaPrimaResponse>> InativarProduto(int id)
        {
            var materiaPrima = await _materiaPrimaService.BuscarMateriaPorIdAsync(id);
            if (materiaPrima == null) return NotFound();
            materiaPrima.Ativo = false;

            await _materiaPrimaService.AtualizarAsync(materiaPrima);
            return Ok(_materiaPrimaService.EntityToResponse(materiaPrima));
        }

        /// <summary>
        /// Cadastrar uma matéria-prima por importação do XML de uma nota fiscal
        /// </summary>
        [HttpPost("ImportarXML")]
        public async Task<ActionResult<MateriaPrimaResponse>> CadastrarMateriaPrimaPorXML(IFormFile arquivoXML)
        {
            var novaMateriaPrima = _materiaPrimaService.CriarMateriaPrimaPorXML(arquivoXML);
            var materiaPrima = await _materiaPrimaService.BuscarMateriaPorIdAsync(novaMateriaPrima.Id);
            return Ok(_materiaPrimaService.EntityToResponse(materiaPrima));
        }
    }
}
