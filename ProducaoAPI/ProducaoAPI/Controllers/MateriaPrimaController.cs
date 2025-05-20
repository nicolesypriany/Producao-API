using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProducaoAPI.Extensions;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
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
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma matéria-prima encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MateriaPrimaResponse>>> ListarMateriasPrimas()
        {
            var materiasPrimas = await _materiaPrimaService.ListarMateriasPrimasAtivas();
            return Ok(materiasPrimas.MapListToResponse());
        }

        /// <summary>
        /// Obter matéria-prima por ID
        /// </summary>
        ///<param name="id">ID da matéria-prima buscada.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma matéria-prima encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<MateriaPrimaResponse>> BuscarMateriaPrimaPorId(int id)
        {
            var materiaPrima = await _materiaPrimaService.BuscarMateriaPorIdAsync(id);
            return Ok(materiaPrima.MapToResponse());
        }

        /// <summary>
        /// Criar uma nova matéria-prima
        /// </summary>
        ///<param name="request">Objeto com os dados da matéria-prima a ser criada.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="500">Erro de servidor</response>
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPost]
        public async Task<ActionResult<MateriaPrimaResponse>> CadastrarMateriaPrima(MateriaPrimaRequest request)
        {
            var materiaPrima = await _materiaPrimaService.AdicionarAsync(request);
            return Ok(materiaPrima.MapToResponse());
        }

        /// <summary>
        /// Atualizar uma matéria-prima
        /// </summary>
        ///<param name="id">ID da matéria-prima a ser atualizada.</param>
        ///<param name="request">Objeto com os dados atualizados da matéria-prima.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma matéria-prima encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPut("{id}")]
        public async Task<ActionResult<MateriaPrimaResponse>> AtualizarMateriaPrima(int id, MateriaPrimaRequest request)
        {
            var materiaPrima = await _materiaPrimaService.AtualizarAsync(id, request);
            return Ok(materiaPrima.MapToResponse());
        }

        /// <summary>
        /// Inativar uma matéria-prima
        /// </summary>
        ///<param name="id">ID da matéria-prima a ser inativada.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhuma matéria-prima encontrada</response>
        ///<response code="500">Erro de servidor</response>
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<MateriaPrimaResponse>> InativarProduto(int id)
        {
            var materiaPrima = await _materiaPrimaService.InativarMateriaPrima(id);
            return Ok(materiaPrima.MapToResponse());
        }

        /// <summary>
        /// Cadastrar uma matéria-prima por importação do XML de uma nota fiscal
        /// </summary>
        ///<param name="arquivoXML">Arquivo XML para importação da matéria-prima.</param>
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPost("ImportarXML")]
        public async Task<ActionResult<MateriaPrimaResponse>> CadastrarMateriaPrimaPorXML(IFormFile arquivoXML)
        {
            try
            {
                var novaMateriaPrima = await _materiaPrimaService.CriarMateriaPrimaPorXML(arquivoXML);
                return Ok(novaMateriaPrima.MapToResponse());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
