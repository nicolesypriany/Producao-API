namespace ProducaoAPI.Requests;

public record ProdutoRequest(string Nome, string Medidas, string Unidade, int PecasPorUnidade);
