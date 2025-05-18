using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Repositories;
using ProducaoAPI.Services.Interfaces;
using ProducaoAPI.Services;

namespace ProducaoAPI
{
    public static class DomainExtensions
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            services.AddScoped<IMaquinaRepository, MaquinaRepository>();
            services.AddScoped<IFormaRepository, FormaRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IMateriaPrimaRepository, MateriaPrimaRepository>();
            services.AddScoped<IProcessoProducaoRepository, ProcessoProducaoRepository>();
            services.AddScoped<IProducaoMateriaPrimaRepository, ProducaoMateriaPrimaRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDespesaRepository, DespesaRepository>();
            services.AddScoped<ILogRepository, LogRepository>();

            services.AddScoped<IFormaService, FormaServices>();
            services.AddScoped<IMaquinaService, MaquinaServices>();
            services.AddScoped<IMateriaPrimaService, MateriaPrimaServices>();
            services.AddScoped<IProdutoService, ProdutoServices>();
            services.AddScoped<IProcessoProducaoService, ProcessoProducaoServices>();
            services.AddScoped<IProducaoMateriaPrimaService, ProducaoMateriaPrimaServices>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFreteService, FreteServices>();
            services.AddScoped<IDespesaService, DespesaService>();
            services.AddScoped<ICustoService, CustoService>();
            services.AddScoped<ILogServices, LogServices>();

            return services;
        }
    }
}
