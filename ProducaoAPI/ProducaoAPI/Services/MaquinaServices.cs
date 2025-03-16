using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;
using ProducaoAPI.Validations;

namespace ProducaoAPI.Services
{
    public class MaquinaServices : IMaquinaService
    {
        private readonly IMaquinaRepository _maquinaRepository;
        public MaquinaServices(IMaquinaRepository maquinaRepository)
        {
            _maquinaRepository = maquinaRepository;
        }
        public MaquinaResponse EntityToResponse(Maquina maquina)
        {
            return new MaquinaResponse(maquina.Id, maquina.Nome, maquina.Marca, maquina.Ativo);
        }
        public ICollection<MaquinaResponse> EntityListToResponseList(IEnumerable<Maquina> maquinas)
        {
            return maquinas.Select(m => EntityToResponse(m)).ToList();
        }

        public Task<IEnumerable<Maquina>> ListarMaquinasAtivas() => _maquinaRepository.ListarMaquinasAtivas();
        public Task<IEnumerable<Maquina>> ListarTodasMaquinas() => _maquinaRepository.ListarTodasMaquinas();

        public Task<Maquina> BuscarMaquinaPorIdAsync(int id) => _maquinaRepository.BuscarMaquinaPorIdAsync(id);

        public async Task<Maquina> AdicionarAsync(MaquinaRequest request)
        {
            await ValidarRequest(true, request);
            var maquina = new Maquina(request.Nome, request.Marca);
            await _maquinaRepository.AdicionarAsync(maquina);
            return maquina;
        }

        public async Task<Maquina> AtualizarAsync(int id, MaquinaRequest request)
        {
            var maquina = await BuscarMaquinaPorIdAsync(id);
            await ValidarRequest(false, request, maquina.Nome);

            maquina.Nome = request.Nome;
            maquina.Marca = request.Marca;

            await _maquinaRepository.AtualizarAsync(maquina);
            return maquina;
        }

        public async Task<Maquina> InativarMaquina(int id)
        {
            var maquina = await BuscarMaquinaPorIdAsync(id);
            maquina.Ativo = false;
            await _maquinaRepository.AtualizarAsync(maquina);
            return maquina;
        }

        private async Task ValidarRequest(bool Cadastrar, MaquinaRequest request, string nomeAtual = "")
        {
            var nomeMaquinas = await _maquinaRepository.ListarNomes();
            
            ValidarCampos.Nome(Cadastrar, nomeMaquinas, request.Nome, nomeAtual);
            ValidarCampos.String(request.Nome, "Nome");
            ValidarCampos.String(request.Marca, "Marca");
        }
    }
}