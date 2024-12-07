namespace ProducaoAPI.Models
{
    public class MateriaPrima
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Fornecedor { get; set; }
        public string Unidade { get; set; }
        public double Preco { get; set; }
        public ICollection<ProcessoProducaoMateriaPrima> ProducaoMateriasPrimas { get; set; }
        public bool Ativo { get; set; }
    }
}
