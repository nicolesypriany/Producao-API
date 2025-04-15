namespace ProducaoAPI.Models
{
    public class ProcessoProducaoMateriaPrima
    {
        public ProcessoProducaoMateriaPrima(int producaoId, int materiaPrimaId, decimal preco, decimal quantidade)
        {
            ProducaoId = producaoId;
            MateriaPrimaId = materiaPrimaId;
            Preco = preco;
            Quantidade = quantidade;
            Ativo = true;
        }

        public int ProducaoId { get; set; }
        public ProcessoProducao ProcessoProducao { get; set; }

        public int MateriaPrimaId { get; set; }
        public MateriaPrima MateriaPrima { get; set; }
        public decimal Preco { get; set; }

        public decimal Quantidade { get; set; }
        public bool Ativo { get; set; }
    }
}
