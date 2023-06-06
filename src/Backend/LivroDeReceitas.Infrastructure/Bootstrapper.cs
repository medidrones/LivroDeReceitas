using System.Reflection;
using FluentMigrator.Runner;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Domain.Repositorios;
using LivroDeReceitas.Infrastructure.AcessoRepositorio;
using LivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using LivroDeReceitas.Domain.Repositorios.Usuario;
using LivroDeReceitas.Domain.Repositorios.Receita;

namespace LivroDeReceitas.Infrastructure;

public static class Bootstrapper
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configurationManager)
    {
        AddFluentMigrator(services, configurationManager);

        AddContexto(services, configurationManager);
        AddUnidadeDeTrabalho(services);
        AddRepositorios(services);
    }

    private static void AddContexto(IServiceCollection services, IConfiguration configurationManager)
    {
        _ = bool.TryParse(configurationManager.GetSection("Configuracoes:BancoDeDadosInMemory").Value, out bool bancoDeDadosInMemory);

        if (!bancoDeDadosInMemory)
        {
            var versaoServidor = new MySqlServerVersion(new Version(8, 0, 33));
            var connectionString = configurationManager.GetConexaoCompleta();

            services.AddDbContext<LivroDeReceitasContext>(dbContextoOpcoes =>
            {
                dbContextoOpcoes.UseMySql(connectionString, versaoServidor);
            });
        }
    }

    private static void AddUnidadeDeTrabalho(IServiceCollection services)
    {
        services.AddScoped<IUnidadeDeTrabalho, UnidadeDeTrabalho>();
    }

    private static void AddRepositorios(IServiceCollection services)
    {
        services
            .AddScoped<IUsuarioWriteOnlyRepositorio, UsuarioRepositorio>()
            .AddScoped<IUsuarioReadOnlyRepositorio, UsuarioRepositorio>()
            .AddScoped<IUsuarioUpdateOnlyRepositorio, UsuarioRepositorio>()
            .AddScoped<IReceitaWriteOnlyRepositorio, ReceitaRepositorio>()
            .AddScoped<IReceitaReadOnlyRepositorio, ReceitaRepositorio>();
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configurationManager)
    {
        _ = bool.TryParse(configurationManager.GetSection("Configuracoes:BancoDeDadosInMemory").Value, out bool bancoDeDadosInMemory);

        if (!bancoDeDadosInMemory)
        {
            services.AddFluentMigratorCore().ConfigureRunner(c =>
                c.AddMySql5()
                    .WithGlobalConnectionString(configurationManager.GetConexaoCompleta()).ScanIn(Assembly.Load("LivroDeReceitas.Infrastructure"))
                    .For.All());
        }
    }
}