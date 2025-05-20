using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Extensions
{
    public static class MaquinaExtension
    {
        public static MaquinaResponse MapToResponse(this Maquina maquina)
        {
            return new MaquinaResponse(
                maquina.Id, 
                maquina.Nome, 
                maquina.Marca, 
                maquina.Ativo
            );
        }

        public static IEnumerable<MaquinaResponse> MapListToResponse(this IEnumerable<Maquina> maquinas)
        {
            return maquinas.Select(m => m.MapToResponse());
        }

        public static Maquina MapToMaquina(this MaquinaRequest request)
        {
            return new Maquina(request.Nome, request.Marca);
        }
    }
}
