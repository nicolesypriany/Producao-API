using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Services
{
    public static class ProcessoProducaoServices
    {
        public static ProcessoProducaoResponse EntityToResponse(ProcessoProducao producao)
        {
            return new ProcessoProducaoResponse(producao.Id, producao.Data, producao.MaquinaId, producao.FormaId, producao.Ciclos, ProducaoMateriaPrimaServices.EntityListToResponseList(producao.ProducaoMateriasPrimas), producao.QuantidadeProduzida, producao.CustoUnitario, producao.CustoTotal, producao.Ativo);
        }
        public static ICollection<ProcessoProducaoResponse> EntityListToResponseList(IEnumerable<ProcessoProducao> producoes)
        {
            return producoes.Select(m => EntityToResponse(m)).ToList();
        }
        public static List<ProcessoProducaoMateriaPrima> CriarProducoesMateriasPrimas(ProducaoContext context, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimas, int ProducaoId)
        {
            var producoesMateriasPrimas = new List<ProcessoProducaoMateriaPrima>();

            foreach (var materiaPrima in materiasPrimas)
            {
                var materiaPrimaSelecionada = context.MateriasPrimas.FirstOrDefault(m => m.Id == materiaPrima.Id);
                var producaoMateriaPrima = new ProcessoProducaoMateriaPrima(ProducaoId, materiaPrimaSelecionada.Id, materiaPrima.Quantidade);
                producoesMateriasPrimas.Add(producaoMateriaPrima);
            }
            return (producoesMateriasPrimas);
        }
    }
}
