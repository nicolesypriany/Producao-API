using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using System.Xml;

namespace ProducaoAPI.Services.Interfaces
{
    public interface IMateriaPrimaService
    {
        Task<IEnumerable<MateriaPrima>> ListarMateriasPrimasAtivas();
        Task<IEnumerable<MateriaPrima>> ListarTodasMateriasPrimas();
        Task<MateriaPrima> BuscarMateriaPorIdAsync(int id);
        Task AdicionarAsync(MateriaPrima materiaPrima);
        Task AtualizarAsync(MateriaPrima materiaPrima);
        MateriaPrimaResponse EntityToResponse(MateriaPrima materiaPrima);
        ICollection<MateriaPrimaResponse> EntityListToResponseList(IEnumerable<MateriaPrima> materiaPrima);
        Task<MateriaPrima> CriarMateriaPrimaPorXML(IFormFile arquivoXML);
        XmlDocument SalvarXML(IFormFile arquivoXML);
        Task ValidarDadosParaCadastrar(MateriaPrimaRequest request);
        Task ValidarDadosParaAtualizar(MateriaPrimaRequest request, int id);
    }
}
