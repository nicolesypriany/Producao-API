using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Services
{
    public class ProducaoMateriaPrimaServices : IProducaoMateriaPrimaService
    {
        private readonly IProducaoMateriaPrimaRepository _producaoMateriaPrimaRepository;
        private readonly ProducaoContext _context;
        public ProducaoMateriaPrimaServices(IProducaoMateriaPrimaRepository producaoMateriaPrimaRepository, ProducaoContext context)
        {
            _producaoMateriaPrimaRepository = producaoMateriaPrimaRepository;
            _context = context;
        }

        public ProducaoMateriaPrimaResponse EntityToResponse(ProcessoProducaoMateriaPrima producaoMateriaPrima)
        {
            return new ProducaoMateriaPrimaResponse(producaoMateriaPrima.MateriaPrimaId, producaoMateriaPrima.MateriaPrima.Nome, producaoMateriaPrima.Quantidade);
        }

        public ICollection<ProducaoMateriaPrimaResponse> EntityListToResponseList(ICollection<ProcessoProducaoMateriaPrima> producoesMateriasPrimas)
        {
            return producoesMateriasPrimas.Select(m => EntityToResponse(m)).ToList();
        }

        public void VerificarProducoesMateriasPrimasExistentes(int producaoId, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest)
        {
            var listaIdMateriasAtuais = new List<int>();
            foreach (var producaoMateriaPrima in _context.ProducoesMateriasPrimas.Where(p => p.ProducaoId == producaoId)) listaIdMateriasAtuais.Add(producaoMateriaPrima.MateriaPrimaId);

            var listaIdNovasMaterias = new List<int>();
            foreach(var producaoMateriaPrimaRequest in materiasPrimasRequest) listaIdNovasMaterias.Add(producaoMateriaPrimaRequest.Id);

            CriarOuAtualizarProducaoMateriaPrima(producaoId, listaIdNovasMaterias, listaIdMateriasAtuais, materiasPrimasRequest);

            ExcluirProducaoMateriaPrima(producaoId, listaIdNovasMaterias, listaIdMateriasAtuais);

        }

        public void CriarOuAtualizarProducaoMateriaPrima(int producaoId, List<int> listaIdNovasMaterias, List<int> listaIdMateriasAtuais, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest)
        {
            foreach (var materiaPrimaId in listaIdNovasMaterias)
            {
                if (listaIdMateriasAtuais.Contains(materiaPrimaId))
                {
                    CompararQuantidadesMateriasPrimas(producaoId, materiaPrimaId, materiasPrimasRequest);
                }
                else
                {
                    var quantidade = RetornarQuantidadeMateriaPrima(materiaPrimaId, materiasPrimasRequest);
                    var novoProcesso = new ProcessoProducaoMateriaPrima(producaoId, materiaPrimaId, quantidade);
                    _context.ProducoesMateriasPrimas.Add(novoProcesso);
                    _context.SaveChanges();
                }
            }
        }

        public void ExcluirProducaoMateriaPrima(int producaoId, List<int> listaIdNovasMaterias, List<int> listaIdMateriasAtuais)
        {
            foreach (var materiaPrimaId in listaIdMateriasAtuais)
            {
                if (!listaIdNovasMaterias.Contains(materiaPrimaId))
                {
                    var producaoMateriaPrimaExistente = _context.ProducoesMateriasPrimas
                        .Where(p => p.ProducaoId == producaoId)
                        .Where(p => p.MateriaPrimaId == materiaPrimaId)
                        .FirstOrDefault();

                    _context.ProducoesMateriasPrimas.Remove(producaoMateriaPrimaExistente);
                    _context.SaveChanges();
                }
            }
        }

        public double RetornarQuantidadeMateriaPrima(int idMateriaPrima, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest)
        {
            double quantidade = 0;
            foreach(var materiaPrima in materiasPrimasRequest)
            {
                if (materiaPrima.Id == idMateriaPrima) quantidade = materiaPrima.Quantidade;
            }
            return quantidade;
        }

        public void CompararQuantidadesMateriasPrimas(int producaoId, int materiaPrimaId, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest)
        {
            var producaoMateriaPrimaExistente = _context.ProducoesMateriasPrimas
                        .Where(p => p.ProducaoId == producaoId)
                        .Where(p => p.MateriaPrimaId == materiaPrimaId)
                        .FirstOrDefault();

            var quantidadeNova = RetornarQuantidadeMateriaPrima(materiaPrimaId, materiasPrimasRequest);
            if (producaoMateriaPrimaExistente.Quantidade != quantidadeNova) producaoMateriaPrimaExistente.Quantidade = quantidadeNova;
            _context.SaveChanges();
        }

        public Task AdicionarAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima) => _producaoMateriaPrimaRepository.AdicionarAsync(producaoMateriaPrima);
    }
}
