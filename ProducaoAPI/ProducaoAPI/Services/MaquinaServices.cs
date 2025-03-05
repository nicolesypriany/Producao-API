using ProducaoAPI.Exceptions;
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
            await ValidarRequest(false, request, id);
            var maquina = await BuscarMaquinaPorIdAsync(id);

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

        private async Task ValidarRequest(bool Cadastrar, MaquinaRequest request, int id = 0)
        {
            var nomeMaquinas = await _maquinaRepository.ListarNomes();

            if (Cadastrar)
            {
                ValidarCampos.NomeParaCadastrarObjeto(nomeMaquinas, request.Nome);
            }
            else
            {
                var maquina = await _maquinaRepository.BuscarMaquinaPorIdAsync(id);
                ValidarCampos.NomeParaAtualizarObjeto(nomeMaquinas, maquina.Nome, request.Nome);
            }

            ValidarCampos.String(request.Nome, "Nome");
            ValidarCampos.String(request.Marca, "Marca");
        }
    }
}