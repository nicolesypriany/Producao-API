using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Services;
using ProducaoAPI.Services.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IMaquinaRepository, MaquinaRepository>();
builder.Services.AddScoped<IFormaRepository, FormaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IMateriaPrimaRepository, MateriaPrimaRepository>();
builder.Services.AddScoped<IProcessoProducaoRepository, ProcessoProducaoRepository>();
builder.Services.AddScoped<IProducaoMateriaPrimaRepository, ProducaoMateriaPrimaRepository>();
builder.Services.AddScoped<IFormaService, FormaServices>();
builder.Services.AddScoped<IMaquinaService, MaquinaServices>();
builder.Services.AddScoped<IMateriaPrimaService, MateriaPrimaServices>();
builder.Services.AddScoped<IProdutoService, ProdutoServices>();
builder.Services.AddScoped<IProcessoProducaoService, ProcessoProducaoServices>();
builder.Services.AddScoped<IProducaoMateriaPrimaService, ProducaoMateriaPrimaServices>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Produção API",
        Description = $"Uma ASP.NET Core Web API para gerenciamento de produções.",
        Contact = new OpenApiContact
        {
            Name = "Respositório",
            Url = new Uri("https://github.com/nicolesypriany/Producao")
        },
    });

    // Habilitando descrições por XML
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddDbContext<ProducaoContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentityApiEndpoints<PessoaComAcesso>()
    .AddEntityFrameworkStores<ProducaoContext>();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:3001", "http://127.0.0.1:3001/") // The URL of your frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowLocalhost");

app.MapControllers();

app.MapGroup("auth").MapIdentityApi<PessoaComAcesso>().WithTags("Autorização");

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.Run();

public partial class Program { }
