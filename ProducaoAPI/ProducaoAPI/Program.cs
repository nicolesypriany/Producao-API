using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Repositories;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Services;
using ProducaoAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDespesaRepository, DespesaRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();

builder.Services.AddScoped<IFormaService, FormaServices>();
builder.Services.AddScoped<IMaquinaService, MaquinaServices>();
builder.Services.AddScoped<IMateriaPrimaService, MateriaPrimaServices>();
builder.Services.AddScoped<IProdutoService, ProdutoServices>();
builder.Services.AddScoped<IProcessoProducaoService, ProcessoProducaoServices>();
builder.Services.AddScoped<IProducaoMateriaPrimaService, ProducaoMateriaPrimaServices>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFreteService, FreteServices>();
builder.Services.AddScoped<IDespesaService, DespesaService>();
builder.Services.AddScoped<ICustoService, CustoService>();
builder.Services.AddScoped<ILogServices, LogServices>();

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
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("RestrictedCors", policy =>
    {
        policy.WithOrigins(
                "https://producao.pro",
                "https://168.231.90.71:5001",
                "http://localhost:3000",
                "http://localhost:5000",
                "http://168.231.90.71:5000"
              )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
            ).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["jwt:issuer"],
                    ValidAudience = builder.Configuration["jwt:audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:secretKey"])),
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"

                };
            });

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(app.Environment.IsDevelopment() ? "AllowAll" : "RestrictedCors");

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.Run();

public partial class Program { }
