using Microsoft.EntityFrameworkCore;
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
        public static void CalcularProducao(ProducaoContext context, int producaoId)
        {
            var producao = context.Producoes
                .Include(p => p.ProducaoMateriasPrimas)
                .ThenInclude(p => p.MateriaPrima)
                .FirstOrDefault(p => p.Id == producaoId);

            var forma = context.Formas.FirstOrDefault(f => f.Id == producao.FormaId);
            var produto = context.Produtos.FirstOrDefault(p => p.Id == producao.ProdutoId);

            double quantidadeProduzida = (producao.Ciclos * forma.PecasPorCiclo) / produto.PecasPorUnidade;

            double custoTotal = 0;
            foreach (var producaoMateriaPrima in producao.ProducaoMateriasPrimas)
            {
                var total = producaoMateriaPrima.Quantidade * producaoMateriaPrima.MateriaPrima.Preco;
                custoTotal += total;
            }

            producao.QuantidadeProduzida = quantidadeProduzida;
            producao.CustoTotal = custoTotal;
            producao.CustoUnitario = custoTotal / quantidadeProduzida;
            context.SaveChanges();
        }
    }
}
