namespace ProducaoAPI.Requests;

public record ProducaoPorProdutoEPeriodoRequest(
    int ProdutoId,
    DateTime DataInicio,
    DateTime DataFim
);
