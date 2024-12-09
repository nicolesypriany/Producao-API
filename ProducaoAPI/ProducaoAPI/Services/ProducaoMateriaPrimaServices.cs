using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Services
{
    public static class ProducaoMateriaPrimaServices
    {
        public static ProducaoMateriaPrimaResponse EntityToResponse(ProcessoProducaoMateriaPrima producaoMateriaPrima)
        {
            return new ProducaoMateriaPrimaResponse(producaoMateriaPrima.MateriaPrimaId, producaoMateriaPrima.MateriaPrima.Nome, producaoMateriaPrima.Quantidade);
        }
        public static ICollection<ProducaoMateriaPrimaResponse> EntityListToResponseList(ICollection<ProcessoProducaoMateriaPrima> producoesMateriasPrimas)
        {
            return producoesMateriasPrimas.Select(m => EntityToResponse(m)).ToList();
        }
        public static void VerificarProducoesMateriasPrimasExistentes()
        {

        }
    }
}
