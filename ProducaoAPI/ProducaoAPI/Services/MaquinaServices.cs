using ProducaoAPI.Models;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Services
{
    public static class MaquinaServices
    {
        public static MaquinaResponse EntityToResponse(Maquina maquina)
        {
            return new MaquinaResponse(maquina.Id, maquina.Nome, maquina.Marca, maquina.Ativo);
        }
        public static ICollection<MaquinaResponse> EntityListToResponseList(IEnumerable<Maquina> maquinas)
        {
            return maquinas.Select(m => EntityToResponse(m)).ToList();
        }
    }
}
