namespace ProducaoAPI.Responses;

public record ProcessoProducaoResponse(int Id, DateTime Data, string Maquina, string Forma, string Produto, int Ciclos, ICollection<ProducaoMateriaPrimaResponse> ProducaoMateriasPrimas, decimal? QuantidadeProduzida, string Unidade, decimal? CustoUnitario, decimal? CustoTotal, DateTime DataCriacao, DateTime? DataEdicao, bool Ativo);