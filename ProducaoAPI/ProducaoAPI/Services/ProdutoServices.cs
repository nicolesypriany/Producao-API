using ProducaoAPI.Models;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Services
{
    public static class ProdutoServices
    {
        public static ProdutoResponse EntityToResponse(Produto produto)
        {
            return new ProdutoResponse(produto.Id, produto.Nome, produto.Medidas, produto.Unidade, produto.PecasPorUnidade, produto.Ativo);
        }
        public static ICollection<ProdutoResponse> EntityListToResponseList(IEnumerable<Produto> produto)
        {
            return produto.Select(f => EntityToResponse(f)).ToList();
        }
    }
}
