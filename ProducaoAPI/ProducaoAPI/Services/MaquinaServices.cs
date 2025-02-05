﻿using ProducaoAPI.Models;
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

        public Task<IEnumerable<Maquina>> ListarMaquinasAsync() => _maquinaRepository.ListarMaquinasAsync();

        public Task<Maquina> BuscarMaquinaPorIdAsync(int id) => _maquinaRepository.BuscarMaquinaPorIdAsync(id);

        public Task AdicionarAsync(Maquina maquina) => _maquinaRepository.AdicionarAsync(maquina);

        public Task AtualizarAsync(Maquina maquina) => _maquinaRepository.AtualizarAsync(maquina);

        public async Task ValidarDados(MaquinaRequest request)
        {
            var maquinas = await _maquinaRepository.ListarMaquinasAsync();
            var nomeMaquinas = new List<string>();
            foreach (var maquina in maquinas)
            {
                nomeMaquinas.Add(maquina.Nome);
            }

            if (nomeMaquinas.Contains(request.Nome)) throw new ArgumentException("Já existe uma máquina com este nome!");
            if (string.IsNullOrWhiteSpace(request.Nome)) throw new ArgumentException("O campo \"Nome\" não pode estar vazio");
            if (string.IsNullOrWhiteSpace(request.Marca)) throw new ArgumentException("O campo \"Marca\" não pode estar vazio");
        }
    }
}
