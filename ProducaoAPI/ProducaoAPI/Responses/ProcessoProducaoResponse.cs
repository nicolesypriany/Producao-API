namespace ProducaoAPI.Responses;

public record ProcessoProducaoResponse(int Id, DateTime Data, int MaquinaId, int FormaId, int Ciclos, ICollection<ProducaoMateriaPrimaResponse> ProducaoMateriasPrimas, double? QuantidadeProduzida, double? CustoUnitario, double? CustoTotal, bool Ativo);