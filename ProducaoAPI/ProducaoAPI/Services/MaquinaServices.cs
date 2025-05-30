﻿using ProducaoAPI.Extensions;
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
        private readonly ILogServices _logServices;

        public MaquinaServices(IMaquinaRepository maquinaRepository, ILogServices logServices)
        {
            _maquinaRepository = maquinaRepository;
            _logServices = logServices;
        }

        public Task<IEnumerable<Maquina>> ListarMaquinasAtivas() => _maquinaRepository.ListarMaquinasAtivas();

        public Task<IEnumerable<Maquina>> ListarTodasMaquinas() => _maquinaRepository.ListarTodasMaquinas();

        public Task<Maquina> BuscarMaquinaPorIdAsync(int id) => _maquinaRepository.BuscarMaquinaPorIdAsync(id);

        public async Task<Maquina> AdicionarAsync(MaquinaRequest request)
        {
            await ValidarRequest(true, request);
            var maquina = request.MapToMaquina();
            await _maquinaRepository.AdicionarAsync(maquina);
            await _logServices.CriarLogAdicionar(typeof(Maquina), maquina.Id);
            return maquina;
        }

        public async Task<Maquina> AtualizarAsync(int id, MaquinaRequest request)
        {
            var maquina = await BuscarMaquinaPorIdAsync(id);
            await ValidarRequest(false, request, maquina.Nome);

            await _logServices.CriarLogAtualizar(
                typeof(Maquina),
                typeof(MaquinaRequest),
                maquina,
                request,
                maquina.Id
            );

            maquina.Nome = request.Nome;
            maquina.Marca = request.Marca;

            await _maquinaRepository.AtualizarAsync(maquina);
            return maquina;
        }

        public async Task<Maquina> InativarMaquina(int id)
        {
            var maquina = await BuscarMaquinaPorIdAsync(id);
            await _logServices.CriarLogInativar(typeof(Maquina), maquina.Id);
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