using HashidsNet;
using LivroDeReceitas.Api.Filtros;
using LivroDeReceitas.Api.Filtros.Swagger;
using LivroDeReceitas.Api.Middleware;
using LivroDeReceitas.Application;
using LivroDeReceitas.Application.Servicos.AutoMapper;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Infrastructure;
using LivroDeReceitas.Infrastructure.AcessoRepositorio;
using LivroDeReceitas.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(option => option.LowercaseUrls = true);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.OperationFilter<HashidsOperationFilter>();
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddMvc(options => options.Filters.Add(typeof(FiltroDasExceptions)));

builder.Services.AddScoped(provider => new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperConfiguracao(provider.GetService<IHashids>()));
}).CreateMapper());

builder.Services.AddScoped<UsuarioAutenticadoAttribute>();

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
