namespace ProducaoAPI.Requests;

public record FormaRequest(string Nome, int ProdutoId, int PecasPorCiclo, ICollection<FormaMaquinaRequest> Maquinas);
