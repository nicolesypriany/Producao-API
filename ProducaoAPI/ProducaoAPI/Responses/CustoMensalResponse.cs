namespace ProducaoAPI.Responses;

public record CustoMensalResponse(
    decimal CustoMensal,
    IEnumerable<DespesaResponse> Despesas,
    IEnumerable<ProcessoProducaoResponse> ProducoesDoMes
);
