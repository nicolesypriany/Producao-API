namespace ProducaoAPI.Responses;

public record CustoMensalResponse(
    decimal? CustoMensal,
    decimal TotalDespesas,
    decimal? CustoTotalProducoes,
    IEnumerable<DespesaResponse> Despesas,
    IEnumerable<ProcessoProducaoResponse> ProducoesDoMes
);