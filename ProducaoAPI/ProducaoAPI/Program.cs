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

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

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

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Produção API",
        Description = "Uma ASP.NET Core Web API para gerenciamento de produções. Para se autenticar, Faça login através do endpoint /User/Login. Copie o Token que será retornado, clique em Authorize e no campo Value preencha com Bearer e o seu token, exemplo: 'Bearer MEU_TOKEN",
        Contact = new OpenApiContact
        {
            Name = "Repositório",
            Url = new Uri("https://github.com/nicolesypriany/Producao")
        },
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = @"Insira o token JWT no campo abaixo.  
Exemplo: **Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...**",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


builder.Services.AddDbContext<ProducaoContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });

    options.AddPolicy("RestrictedCors", policy =>
    {
        policy.WithOrigins(
                "https://producao.pro",
                "https://www.producao.pro",
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

app.UsePathBase("/api");

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger(c =>
{
    c.RouteTemplate = "swagger/{documentName}/swagger.json";
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Produção API");
    c.RoutePrefix = "swagger";
});

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