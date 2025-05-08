namespace ProducaoAPI.Responses;

public record FormaResponse(int Id, string Nome, ProdutoResponse Produto, int PecasPorCiclo, bool Ativo);
