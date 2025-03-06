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
        Task<MateriaPrima> AdicionarAsync(MateriaPrimaRequest request);
        Task<MateriaPrima> AtualizarAsync(int id, MateriaPrimaRequest request);
        Task<MateriaPrima> InativarMateriaPrima(int id);
        MateriaPrimaResponse EntityToResponse(MateriaPrima materiaPrima);
        ICollection<MateriaPrimaResponse> EntityListToResponseList(IEnumerable<MateriaPrima> materiaPrima);
        Task<MateriaPrima> CriarMateriaPrimaPorXML(IFormFile arquivoXML);
        XmlDocument ConverterIFormFileParaXmlDocument(IFormFile arquivoXML);
    }
}
