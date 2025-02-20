using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;

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
            await ValidarDadosParaCadastrar(request);
            var maquina = new Maquina(request.Nome, request.Marca);
            await _maquinaRepository.AdicionarAsync(maquina);
            return maquina;
        }

        public async Task<Maquina> AtualizarAsync(int id, MaquinaRequest request)
        {
            try
            {
                await ValidarDadosParaAtualizar(request, id);
                var maquina = await BuscarMaquinaPorIdAsync(id);

                maquina.Nome = request.Nome;
                maquina.Marca = request.Marca;

                await _maquinaRepository.AtualizarAsync(maquina);
                return maquina;
            }
            catch (BadRequestException)
            {
                throw;
            }
        }

        public async Task<Maquina> InativarMaquina(int id)
        {
            var maquina = await BuscarMaquinaPorIdAsync(id);
            maquina.Ativo = false;
            await _maquinaRepository.AtualizarAsync(maquina);
            return maquina;
        }

        public async Task ValidarDadosParaCadastrar(MaquinaRequest request)
        {
            var maquinas = await _maquinaRepository.ListarTodasMaquinas();
            var nomeMaquinas = new List<string>();
            foreach (var maquina in maquinas)
            {
                nomeMaquinas.Add(maquina.Nome);
            }

            if (nomeMaquinas.Contains(request.Nome)) throw new ArgumentException("Já existe uma máquina com este nome!");
            if (string.IsNullOrWhiteSpace(request.Nome)) throw new ArgumentException("O campo \"Nome\" não pode estar vazio");
            if (string.IsNullOrWhiteSpace(request.Marca)) throw new ArgumentException("O campo \"Marca\" não pode estar vazio");
        }
        public async Task ValidarDadosParaAtualizar(MaquinaRequest request, int id)
        {
            var maquinaAtualizada = await _maquinaRepository.BuscarMaquinaPorIdAsync(id);

            var maquinas = await _maquinaRepository.ListarTodasMaquinas();
            var nomeMaquinas = new List<string>();
            foreach (var maquina in maquinas)
            {
                nomeMaquinas.Add(maquina.Nome);
            }

            if (nomeMaquinas.Contains(request.Nome) && maquinaAtualizada.Nome != request.Nome) throw new ArgumentException("Já existe uma máquina com este nome!");

            if (string.IsNullOrWhiteSpace(request.Nome)) throw new ArgumentException("O campo \"Nome\" não pode estar vazio");
            if (string.IsNullOrWhiteSpace(request.Marca)) throw new ArgumentException("O campo \"Marca\" não pode estar vazio");
        }
    }
}