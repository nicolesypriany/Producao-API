using Microsoft.AspNetCore.Mvc;
using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;
using System.Xml;

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
        ///<response code="200">Sucesso</response>
        ///<response code="404">Nenhuma matéria-prima encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MateriaPrimaResponse>>> ListarMateriasPrimas()
        {
            var materiasPrimas = await _materiaPrimaService.ListarMateriasPrimasAtivas();
            return Ok(_materiaPrimaService.EntityListToResponseList(materiasPrimas));
        }

        /// <summary>
        /// Obter matéria-prima por ID
        /// </summary>
        ///<response code="200">Sucesso</response>
        ///<response code="404">Nenhuma matéria-prima encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<MateriaPrimaResponse>> BuscarMateriaPrimaPorId(int id)
        {
            var materiaPrima = await _materiaPrimaService.BuscarMateriaPorIdAsync(id);
            return Ok(_materiaPrimaService.EntityToResponse(materiaPrima));
        }

        /// <summary>
        /// Criar uma nova matéria-prima
        /// </summary>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="500">Erro de servidor</response>
        [HttpPost]
        public async Task<ActionResult<MateriaPrimaResponse>> CadastrarMateriaPrima(MateriaPrimaRequest request)
        {
            var materiaPrima = await _materiaPrimaService.AdicionarAsync(request);
            return Ok(_materiaPrimaService.EntityToResponse(materiaPrima));
        }

        /// <summary>
        /// Atualizar uma matéria-prima
        /// </summary>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="404">Nenhuma matéria-prima encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<MateriaPrimaResponse>> AtualizarMateriaPrima(int id, MateriaPrimaRequest request)
        {
            var materiaPrima = await _materiaPrimaService.AtualizarAsync(id, request);
            return Ok(_materiaPrimaService.EntityToResponse(materiaPrima));
        }

        /// <summary>
        /// Inativar uma matéria-prima
        /// </summary>
        ///<response code="200">Sucesso</response>
        ///<response code="404">Nenhuma matéria-prima encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<MateriaPrimaResponse>> InativarProduto(int id)
        {
            var materiaPrima = await _materiaPrimaService.InativarMateriaPrima(id);
            return Ok(_materiaPrimaService.EntityToResponse(materiaPrima));
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
                //var materiaPrima = await _materiaPrimaService.BuscarMateriaPorIdAsync(novaMateriaPrima.Id);
                return Ok(_materiaPrimaService.EntityToResponse(novaMateriaPrima));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
