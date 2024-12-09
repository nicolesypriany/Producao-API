namespace ProducaoAPI.Requests;

public record MateriaPrimaRequest(string Nome, string Fornecedor, string Unidade, double Preco);