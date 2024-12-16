using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Services
{
    public static class FormaServices
    {
        public static FormaResponse EntityToResponse(Forma forma)
        {
            return new FormaResponse(forma.Id, forma.Nome, ProdutoServices.EntityToResponse(forma.Produto), forma.PecasPorCiclo, MaquinaServices.EntityListToResponseList(forma.Maquinas), forma.Ativo);
        }
        public static ICollection<FormaResponse> EntityListToResponseList(IEnumerable<Forma> forma)
        {
            return forma.Select(f => EntityToResponse(f)).ToList();
        }
        public static List<Maquina> FormaMaquinaRequestToEntity(ICollection<FormaMaquinaRequest> maquinas, ProducaoContext context)
        {
            var maquinasSelecionadas = new List<Maquina>();

            foreach(var maquina in maquinas)
            {
                var maquinaSelecionada = context.Maquinas.FirstOrDefault(m => m.Id == maquina.Id);
                maquinasSelecionadas.Add(maquinaSelecionada);
            }

            return maquinasSelecionadas;
        }
    }
}
