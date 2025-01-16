namespace ProducaoAPI.Models
{
    public class ProcessoProducao
    {
        public ProcessoProducao(DateTime data, int maquinaId, int formaId, int produtoId, int ciclos)
        {
            Data = data;
            MaquinaId = maquinaId;
            FormaId = formaId;
            ProdutoId = produtoId;
            Ciclos = ciclos;
            Ativo = true;
        }

        public int Id { get; set; }
        public DateTime Data { get; set; }
        public int MaquinaId { get; set; }
        public Maquina Maquina { get; set; }
        public int FormaId { get; set; }
        public Forma Forma { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int Ciclos { get; set; }
        public double? QuantidadeProduzida { get; set; }
        public ICollection<ProcessoProducaoMateriaPrima> ProducaoMateriasPrimas { get; set; }
        public double? CustoUnitario { get; set; }
        public double? CustoTotal { get; set; }
        public bool Ativo { get; set; }
    }
}
