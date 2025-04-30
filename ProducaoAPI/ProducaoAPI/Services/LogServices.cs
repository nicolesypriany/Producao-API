using ProducaoAPI.Enums;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Services
{
    public class LogServices : ILogServices
    {
        private readonly ILogRepository _logRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly int UserId;

        public LogServices(ILogRepository logRepository, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _logRepository = logRepository;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            UserId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value);
        }

        public async Task<IEnumerable<LogResponse>> BuscarLogs(LogRequest request)
        {
            var logs = await _logRepository.BuscarLogs(request);
            return await EntityListToResponseList(logs);
        }

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

        private async Task<IEnumerable<LogResponse>> EntityListToResponseList(IEnumerable<Log> logs)
        {
            List<LogResponse> logResponses = [];
            foreach (var l in logs)
            {
                var usuario = await _userService.BuscarPorId(l.UserId);
                logResponses.Add(new LogResponse(
                    l.Id,
                    l.Acao,
                    l.Objeto,
                    l.IdObjeto,
                    usuario.Nome,
                    l.Data,
                    l.DadoAlterado,
                    l.Conteudo
                ));
            }
            return logResponses;
        }
    }
}