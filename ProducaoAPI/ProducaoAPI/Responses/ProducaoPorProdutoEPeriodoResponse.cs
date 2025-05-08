namespace ProducaoAPI.Responses;

public record ProducaoPorProdutoEPeriodoResponse(
    int ProdutoId,
    DateTime DataInicio,
    DateTime DataFim,
    decimal? CustoMedio,
    ICollection<ProcessoProducaoResponse> Producoes
);
