namespace ProducaoAPI.Requests;

public record FreteRequest(string EnderecoOrigem, string EnderecoDestino, double PrecoDiesel, double KmPorLitro, int ProdutoId, double QuantidadeProduto, double QuantidadePorPalete, int PaletesPorCarga);