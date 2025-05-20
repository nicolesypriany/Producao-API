using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Extensions
{
    public static class ProdutoExtensions
    {
        public static ProdutoResponse MapToResponse(this Produto produto)
        {
            return new ProdutoResponse(
                produto.Id,
                produto.Nome,
                produto.Medidas,
                produto.Unidade,
                produto.PecasPorUnidade,
                produto.Ativo
            );
        }

        public static IEnumerable<ProdutoResponse> MapListToResponse(this IEnumerable<Produto> produtos)
        {
            return produtos.Select(p => p.MapToResponse());
        }

        public static Produto MapToProduto(this ProdutoRequest request)
        {
            return new Produto(
                request.Nome,
                request.Medidas,
                request.Unidade,
                request.PecasPorUnidade
            );
        }
    }
}
