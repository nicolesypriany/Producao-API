using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Extensions
{
    public static class MateriaPrimaExtensions
    {
        public static MateriaPrimaResponse MapToResponse(this MateriaPrima materiaPrima)
        {
            return new MateriaPrimaResponse(
                materiaPrima.Id,
                materiaPrima.Nome,
                materiaPrima.Fornecedor,
                materiaPrima.Unidade,
                materiaPrima.Preco,
                materiaPrima.Ativo
            );
        }

        public static IEnumerable<MateriaPrimaResponse> MapListToResponse(this IEnumerable<MateriaPrima> materiaPrimas)
        {
            return materiaPrimas.Select(m => m.MapToResponse());
        }

        public static MateriaPrima MapToMateriaPrima(this MateriaPrimaRequest request)
        {
            return new MateriaPrima(
                request.Nome,
                request.Fornecedor,
                request.Unidade,
                request.Preco
            );
        }
    }
}
