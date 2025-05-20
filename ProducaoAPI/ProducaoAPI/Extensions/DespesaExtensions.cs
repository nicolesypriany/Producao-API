using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Extensions
{
    public static class DespesaExtensions
    {
        public static DespesaResponse MapToResponse(this Despesa despesa)
        {
            return new DespesaResponse(
                despesa.Id,
                despesa.Nome,
                despesa.Descricao,
                despesa.Valor
            );
        }

        public static IEnumerable<DespesaResponse> MapListToResponse(this IEnumerable<Despesa> despesas)
        {
            return despesas.Select(d => d.MapToResponse());
        }

        public static Despesa MapToDespesa(this DespesaRequest request)
        {
            return new Despesa(
                request.Nome,
                request.Descricao,
                request.Valor
            );
        }
    }
}
