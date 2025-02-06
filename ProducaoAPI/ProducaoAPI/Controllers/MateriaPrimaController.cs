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
            try
            {
                var materiasPrimas = await _materiaPrimaService.ListarMateriasAsync();
                return Ok(_materiaPrimaService.EntityListToResponseList(materiasPrimas));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Obter matéria-prima por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MateriaPrimaResponse>> BuscarMateriaPrimaPorId(int id)
        {
            try
            {
                var materiaPrima = await _materiaPrimaService.BuscarMateriaPorIdAsync(id);
                return Ok(_materiaPrimaService.EntityToResponse(materiaPrima));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Criar uma nova matéria-prima
        /// </summary>
        /// <response code="200">Matéria-prima cadastrada com sucesso</response>
        /// <response code="400">Request incorreto</response>
        [HttpPost]
        public async Task<ActionResult<MateriaPrimaResponse>> CadastrarMateriaPrima(MateriaPrimaRequest request)
        {
            try
            {
                await _materiaPrimaService.ValidarDados(request);
                var materiaPrima = new MateriaPrima(request.Nome, request.Fornecedor, request.Unidade, request.Preco);
                await _materiaPrimaService.AdicionarAsync(materiaPrima);
                return Ok(_materiaPrimaService.EntityToResponse(materiaPrima));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Atualizar uma matéria-prima
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<MateriaPrimaResponse>> AtualizarMateriaPrima(int id, MateriaPrimaRequest request)
        {
            try
            {
                await _materiaPrimaService.ValidarDados(request);
                var materiaPrima = await _materiaPrimaService.BuscarMateriaPorIdAsync(id);

                materiaPrima.Nome = request.Nome;
                materiaPrima.Fornecedor = request.Fornecedor;
                materiaPrima.Unidade = request.Unidade;
                materiaPrima.Preco = request.Preco;

                await _materiaPrimaService.AtualizarAsync(materiaPrima);
                return Ok(_materiaPrimaService.EntityToResponse(materiaPrima));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Inativar uma matéria-prima
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<MateriaPrimaResponse>> InativarProduto(int id)
        {
            try
            {
                var materiaPrima = await _materiaPrimaService.BuscarMateriaPorIdAsync(id);
                materiaPrima.Ativo = false;

                await _materiaPrimaService.AtualizarAsync(materiaPrima);
                return Ok(_materiaPrimaService.EntityToResponse(materiaPrima));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Cadastrar uma matéria-prima por importação do XML de uma nota fiscal
        /// </summary>
        [HttpPost("ImportarXML")]
        public async Task<ActionResult<MateriaPrimaResponse>> CadastrarMateriaPrimaPorXML(IFormFile arquivoXML)
        {
            try
            {
                var novaMateriaPrima = await _materiaPrimaService.CriarMateriaPrimaPorXML(arquivoXML);
                var materiaPrima = await _materiaPrimaService.BuscarMateriaPorIdAsync(novaMateriaPrima.Id);
                return Ok(_materiaPrimaService.EntityToResponse(materiaPrima));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
