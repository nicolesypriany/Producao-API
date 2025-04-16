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
        private readonly IMateriaPrimaRepository _materiaPrimaRepository;
        public ProducaoMateriaPrimaServices(IProducaoMateriaPrimaRepository producaoMateriaPrimaRepository, IMateriaPrimaRepository materiaPrimaRepository)
        {
            _producaoMateriaPrimaRepository = producaoMateriaPrimaRepository;
            _materiaPrimaRepository = materiaPrimaRepository;
        }

        public ProducaoMateriaPrimaResponse EntityToResponse(ProcessoProducaoMateriaPrima producaoMateriaPrima)
        {
            return new ProducaoMateriaPrimaResponse(producaoMateriaPrima.MateriaPrimaId, producaoMateriaPrima.MateriaPrima.Nome, producaoMateriaPrima.Quantidade, producaoMateriaPrima.Preco);
        }

        public async Task<ICollection<ProducaoMateriaPrimaResponse>> EntityListToResponseList(ICollection<ProcessoProducaoMateriaPrima> producoesMateriasPrimas)
        {
            return producoesMateriasPrimas.Select(m => EntityToResponse(m)).ToList();
        }

        public async Task VerificarProducoesMateriasPrimasExistentes(int producaoId, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest)
        {
            var listaIdMateriasAtuais = new List<int>();
            var producoesMateriasPrimas = await _producaoMateriaPrimaRepository.ListarProcessosProducaoMateriaPrima(producaoId);
            foreach (var producaoMateriaPrima in producoesMateriasPrimas) listaIdMateriasAtuais.Add(producaoMateriaPrima.MateriaPrimaId);

            var listaIdNovasMaterias = new List<int>();
            foreach (var producaoMateriaPrimaRequest in materiasPrimasRequest) listaIdNovasMaterias.Add(producaoMateriaPrimaRequest.Id);

            await CriarOuAtualizarProducaoMateriaPrima(producaoId, listaIdNovasMaterias, listaIdMateriasAtuais, materiasPrimasRequest);
            await ExcluirProducaoMateriaPrima(producaoId, listaIdNovasMaterias, listaIdMateriasAtuais);

        }

        public async Task CriarOuAtualizarProducaoMateriaPrima(int producaoId, List<int> listaIdNovasMaterias, List<int> listaIdMateriasAtuais, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest)
        {
            foreach (var materiaPrimaId in listaIdNovasMaterias)
            {
                if (listaIdMateriasAtuais.Contains(materiaPrimaId))
                {
                    await CompararQuantidadesMateriasPrimas(producaoId, materiaPrimaId, materiasPrimasRequest);
                }
                else
                {
                    var quantidade = await RetornarQuantidadeMateriaPrima(materiaPrimaId, materiasPrimasRequest);
                    var materiaPrima = await _materiaPrimaRepository.BuscarMateriaPrimaPorIdAsync(materiaPrimaId);
                    var novoProcesso = new ProcessoProducaoMateriaPrima(producaoId, materiaPrimaId, materiaPrima.Preco, quantidade);
                    await _producaoMateriaPrimaRepository.AdicionarAsync(novoProcesso);
                }
            }
        }

        public async Task ExcluirProducaoMateriaPrima(int producaoId, List<int> listaIdNovasMaterias, List<int> listaIdMateriasAtuais)
        {
            foreach (var materiaPrimaId in listaIdMateriasAtuais)
            {
                if (!listaIdNovasMaterias.Contains(materiaPrimaId))
                {
                    var producaoMateriaPrimaExistente = await _producaoMateriaPrimaRepository.BuscarProcessoProducaoMateriaPrima(producaoId, materiaPrimaId);
                    await _producaoMateriaPrimaRepository.RemoverAsync(producaoMateriaPrimaExistente);
                }
            }
        }

        public async Task<decimal> RetornarQuantidadeMateriaPrima(int idMateriaPrima, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest)
        {
            decimal quantidade = 0;
            foreach (var materiaPrima in materiasPrimasRequest)
            {
                if (materiaPrima.Id == idMateriaPrima) quantidade = materiaPrima.Quantidade;
            }
            return quantidade;
        }

        public async Task CompararQuantidadesMateriasPrimas(int producaoId, int materiaPrimaId, ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimasRequest)
        {
            var producaoMateriaPrimaExistente = await _producaoMateriaPrimaRepository.BuscarProcessoProducaoMateriaPrima(producaoId, materiaPrimaId);
            var quantidadeNova = await RetornarQuantidadeMateriaPrima(materiaPrimaId, materiasPrimasRequest);
            if (producaoMateriaPrimaExistente.Quantidade != quantidadeNova) producaoMateriaPrimaExistente.Quantidade = quantidadeNova;
            await _producaoMateriaPrimaRepository.AtualizarAsync(producaoMateriaPrimaExistente);
        }

        public Task AdicionarAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima) => _producaoMateriaPrimaRepository.AdicionarAsync(producaoMateriaPrima);
    }
}
