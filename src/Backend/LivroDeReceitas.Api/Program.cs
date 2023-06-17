using HashidsNet;
using LivroDeReceitas.Api.Filtros;
using LivroDeReceitas.Api.Filtros.Swagger;
using LivroDeReceitas.Api.Filtros.UsuarioLogado;
using LivroDeReceitas.Api.Middleware;
using LivroDeReceitas.Api.WebSockets;
using LivroDeReceitas.Application;
using LivroDeReceitas.Application.Servicos.AutoMapper;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Infrastructure;
using LivroDeReceitas.Infrastructure.AcessoRepositorio;
using LivroDeReceitas.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(option => option.LowercaseUrls = true);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.OperationFilter<HashidsOperationFilter>();
    option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Livro de Receitas API", Version = "1.0" });
    option.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header utilizando o Bearer scheme. Example: \"Authorization: Bearer {token}\""
    });
    option.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            System.Array.Empty<string>()
        }
    });
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddMvc(options => options.Filters.Add(typeof(FiltroDasExceptions)));

builder.Services.AddScoped(provider => new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperConfiguracao(provider.GetService<IHashids>()));
}).CreateMapper());

builder.Services.AddScoped<IAuthorizationHandler, UsuarioLogadoHandler>();

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("UsuarioLogado", policy => policy.Requirements.Add(new UsuarioLogadoRequirement()));
});

builder.Services.AddScoped<UsuarioAutenticadoAttribute>();
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

AtualizarBaseDeDados();

app.UseMiddleware<CultureMiddleware>();
app.MapHub<AdicionarConexao>("/addConexao");

app.Run();

void AtualizarBaseDeDados()
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    using var context = serviceScope.ServiceProvider.GetService<LivroDeReceitasContext>();

    bool? databaseInMemory = context?.Database?.ProviderName?.Equals("Microsoft.EntityFrameworkCore.InMemory");

    if (!databaseInMemory.HasValue || !databaseInMemory.Value)
    {
        var conexao = builder.Configuration.GetConexao();
        var nomeDatabase = builder.Configuration.GetNomeDatabase();

        Database.CriarDatabase(conexao, nomeDatabase);

        app.MigrateBancoDeDados();
    }
}

#pragma warning disable CA1050, S3903, S1118
public partial class Program
{
}
#pragma warning restore CA1050, S3903, S1118
