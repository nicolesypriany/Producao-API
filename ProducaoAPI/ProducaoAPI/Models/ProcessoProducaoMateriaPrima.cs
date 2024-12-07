namespace ProducaoAPI.Models
{
    public class ProcessoProducaoMateriaPrima
    {
        public int ProducaoId { get; set; }
        public ProcessoProducao ProcessoProducao { get; set; }

        public int MateriaPrimaId { get; set; }
        public MateriaPrima MateriaPrima { get; set; }

        public double Quantidade { get; set; }
        public bool Ativo { get; set; }
    }
}
