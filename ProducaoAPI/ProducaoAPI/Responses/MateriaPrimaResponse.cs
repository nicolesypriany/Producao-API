namespace ProducaoAPI.Responses;

public record MateriaPrimaResponse(int Id, string Nome, string Fornecedor, string Unidade, decimal Preco, bool Ativo);
