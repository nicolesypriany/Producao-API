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



        public static void VerificarProducoesMateriasPrimasExistentes(ProducaoContext context, int producaoId, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest)
        {
            var listaIdMateriasAtuais = new List<int>();
            foreach (var producaoMateriaPrima in context.ProducoesMateriasPrimas.Where(p => p.ProducaoId == producaoId)) listaIdMateriasAtuais.Add(producaoMateriaPrima.MateriaPrimaId);

            var listaIdNovasMaterias = new List<int>();
            foreach(var producaoMateriaPrimaRequest in materiasPrimasRequest) listaIdNovasMaterias.Add(producaoMateriaPrimaRequest.Id);

            CriarOuAtualizarProducaoMateriaPrima(context, producaoId, listaIdNovasMaterias, listaIdMateriasAtuais, materiasPrimasRequest);

            ExcluirProducaoMateriaPrima(context, producaoId, listaIdNovasMaterias, listaIdMateriasAtuais);

        }

        private static void CriarOuAtualizarProducaoMateriaPrima(ProducaoContext context, int producaoId, List<int> listaIdNovasMaterias, List<int> listaIdMateriasAtuais, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest)
        {
            foreach (var materiaPrimaId in listaIdNovasMaterias)
            {
                if (listaIdMateriasAtuais.Contains(materiaPrimaId))
                {
                    CompararQuantidadesMateriasPrimas(context, producaoId, materiaPrimaId, materiasPrimasRequest);
                }
                else
                {
                    var quantidade = RetornarQuantidadeMateriaPrima(materiaPrimaId, materiasPrimasRequest);
                    var novoProcesso = new ProcessoProducaoMateriaPrima(producaoId, materiaPrimaId, quantidade);
                    context.ProducoesMateriasPrimas.Add(novoProcesso);
                    context.SaveChanges();
                }
            }
        }

        private static void ExcluirProducaoMateriaPrima(ProducaoContext context, int producaoId, List<int> listaIdNovasMaterias, List<int> listaIdMateriasAtuais)
        {
            foreach (var materiaPrimaId in listaIdMateriasAtuais)
            {
                if (!listaIdNovasMaterias.Contains(materiaPrimaId))
                {
                    var producaoMateriaPrimaExistente = context.ProducoesMateriasPrimas
                        .Where(p => p.ProducaoId == producaoId)
                        .Where(p => p.MateriaPrimaId == materiaPrimaId)
                        .FirstOrDefault();

                    context.ProducoesMateriasPrimas.Remove(producaoMateriaPrimaExistente);
                    context.SaveChanges();
                }
            }
        }

        private static double RetornarQuantidadeMateriaPrima(int idMateriaPrima, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest)
        {
            double quantidade = 0;
            foreach(var materiaPrima in materiasPrimasRequest)
            {
                if (materiaPrima.Id == idMateriaPrima) quantidade = materiaPrima.Quantidade;
            }
            return quantidade;
        }

        private static void CompararQuantidadesMateriasPrimas(ProducaoContext context, int producaoId, int materiaPrimaId, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest)
        {
            var producaoMateriaPrimaExistente = context.ProducoesMateriasPrimas
                        .Where(p => p.ProducaoId == producaoId)
                        .Where(p => p.MateriaPrimaId == materiaPrimaId)
                        .FirstOrDefault();

            var quantidadeNova = RetornarQuantidadeMateriaPrima(materiaPrimaId, materiasPrimasRequest);
            if (producaoMateriaPrimaExistente.Quantidade != quantidadeNova) producaoMateriaPrimaExistente.Quantidade = quantidadeNova;
            context.SaveChanges();
        }
    }
}
