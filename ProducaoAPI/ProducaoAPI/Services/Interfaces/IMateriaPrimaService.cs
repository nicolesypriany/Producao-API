using ProducaoAPI.Models;
using ProducaoAPI.Requests;
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
        Task<MateriaPrima> CriarMateriaPrimaPorXML(IFormFile arquivoXML);
        XmlDocument ConverterIFormFileParaXmlDocument(IFormFile arquivoXML);
    }
}
