using ProducaoAPI.Enums;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Services
{
    public class LogServices : ILogServices
    {
        private readonly ILogRepository _logRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int UserId;

        public LogServices(ILogRepository logRepository, IHttpContextAccessor httpContextAccessor)
        {
            _logRepository = logRepository;
            _httpContextAccessor = httpContextAccessor;
            UserId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value);
        }

        public Task<IEnumerable<Log>> BuscarLogs(LogRequest request) => _logRepository.BuscarLogs(request);

        public async Task CriarLogAdicionar(Type tipoObjeto, int objetoId)
        {
            await _logRepository.AdicionarAsync(new Log(
                nameof(Acoes.Criar),
                tipoObjeto.Name,
                objetoId,
                UserId
            ));
        }

        public async Task CriarLogAtualizar(Type tipoObjeto, Type tipoRequest, Object objeto, Object request, int objetoId)
        {
            foreach (var propriedadeEntidade in tipoObjeto.GetProperties())
            {
                var propriedadeRequest = tipoRequest.GetProperty(propriedadeEntidade.Name);
                if (propriedadeRequest is null) continue;
                var valorAtual = propriedadeEntidade.GetValue(objeto)?.ToString();
                var novoValor = propriedadeRequest.GetValue(request)?.ToString();

                if (valorAtual != novoValor)
                {
                    await _logRepository.AdicionarAsync(new Log(
                        nameof(Acoes.Editar),
                        tipoObjeto.Name,
                        objetoId,
                        UserId,
                        propriedadeEntidade.Name,
                        novoValor
                    ));
                }
            }
        }

        public async Task CriarLogInativar(Type tipoObjeto, int objetoId)
        {
            await _logRepository.AdicionarAsync(new Log(
                nameof(Acoes.Inativar),
                tipoObjeto.Name,
                objetoId,
                UserId
            ));
        }
    }
}
