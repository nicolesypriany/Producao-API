using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;
using ProducaoAPI.Validations;

namespace ProducaoAPI.Services
{
    public class DespesaService : IDespesaService
    {
        private readonly IDespesaRepository _despesaRepository;

        public DespesaService(IDespesaRepository despesaRepository)
        {
            _despesaRepository = despesaRepository;
        }

        public DespesaResponse EntityToResponse(Despesa despesa)
        {
            return new DespesaResponse(
                despesa.Id,
                despesa.Nome,
                despesa.Descricao,
                despesa.Valor
            );
        }

        public ICollection<DespesaResponse> EntityListToResponseList(IEnumerable<Despesa> despesas)
        {
            return despesas.Select(d => EntityToResponse(d)).ToList();
        }

        public async Task<IEnumerable<Despesa>> ListarDespesasAtivas() => await _despesaRepository.ListarDespesasAtivas();

        public async Task<Despesa> BuscarDespesaPorIdAsync(int id) => await _despesaRepository.BuscarDespesaPorIdAsync(id);

        public async Task<Despesa> AdicionarAsync(DespesaRequest request)
        {
            await ValidarRequest(true, request);
            var despesa = new Despesa(
                request.Nome,
                request.Descricao,
                request.Valor
            );

            await _despesaRepository.AdicionarAsync(despesa);
            return despesa;
        }

        public async Task<Despesa> AtualizarAsync(int id, DespesaRequest request)
        {
            var despesa = await _despesaRepository.BuscarDespesaPorIdAsync(id);
            await ValidarRequest(false, request, despesa.Nome);

            despesa.Nome = request.Nome;
            despesa.Descricao = request.Descricao;
            despesa.Valor = request.Valor;
            
            await _despesaRepository.AtualizarAsync(despesa);
            return despesa;
        }

        public async Task<Despesa> InativarDespesa(int id)
        {
            var despesa = await _despesaRepository.BuscarDespesaPorIdAsync(id);
            despesa.Ativo = false;
            await _despesaRepository.AtualizarAsync(despesa);
            return despesa;
        }

        private async Task ValidarRequest(bool Cadastrar, DespesaRequest request, string nomeAtual = "") 
        {
            var nomeDespesas = await _despesaRepository.ListarNomes();

            ValidarCampos.Nome(Cadastrar, nomeDespesas, request.Nome, nomeAtual);
            ValidarCampos.String(request.Descricao, "Descrição");
            ValidarCampos.Decimal(request.Valor, "Valor");
        }
    }
}
