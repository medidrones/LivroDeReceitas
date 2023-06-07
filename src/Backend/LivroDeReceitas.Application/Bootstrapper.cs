﻿using LivroDeReceitas.Application.Servicos.Criptografia;
using LivroDeReceitas.Application.Servicos.Token;
using LivroDeReceitas.Application.Servicos.UsuarioLogado;
using LivroDeReceitas.Application.UseCases.Dashboard;
using LivroDeReceitas.Application.UseCases.Login.FazerLogin;
using LivroDeReceitas.Application.UseCases.Receita.Atualizar;
using LivroDeReceitas.Application.UseCases.Receita.RecuperarPorId;
using LivroDeReceitas.Application.UseCases.Receita.Registrar;
using LivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;
using LivroDeReceitas.Application.UseCases.Usuario.Registrar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LivroDeReceitas.Application;

public static class Bootstrapper
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AdicionarChaveAdicionalSenha(services, configuration);
        AdicionarHashIds(services, configuration);
        AdicionarTokenJWT(services, configuration);
        AdicionarUseCase(services);
        AdicionarUsuarioLogado(services);
    }

    private static void AdicionarUsuarioLogado(IServiceCollection services)
    {
        services.AddScoped<IUsuarioLogado, UsuarioLogado>();
    }

    private static void AdicionarChaveAdicionalSenha(IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection("Configuracoes:Senha:ChaveAdicionalSenha");

        services.AddScoped(option => new EncriptadorDeSenha(section.Value));
    }

    private static void AdicionarTokenJWT(IServiceCollection services, IConfiguration configuration)
    {
        var sectionTempoDeVida = configuration.GetRequiredSection("Configuracoes:Jwt:TempoVidaTokenMinutos");
        var sectionKey = configuration.GetRequiredSection("Configuracoes:Jwt:ChaveToken");

        services.AddScoped(option => new TokenController(int.Parse(sectionTempoDeVida.Value), sectionKey.Value));
    }

    private static void AdicionarHashIds(IServiceCollection services, IConfiguration configuration)
    {
        var salt = configuration.GetRequiredSection("Configuracoes:HashIds:Salt");

        services.AddHashids(setup =>
        {
            setup.Salt = salt.Value;
            setup.MinHashLength = 3;
        });
    }

    private static void AdicionarUseCase(IServiceCollection services)
    {
        services.AddScoped<IRegistrarUsuarioUseCase, RegistrarUsuarioUseCase>()
            .AddScoped<ILoginUseCase, LoginUseCase>()
            .AddScoped<IAlterarSenhaUseCase, AlterarSenhaUseCase>()
            .AddScoped<IRegistrarReceitaUseCase, RegistrarReceitaUseCase>()
            .AddScoped<IDashboardUseCase, DashboardUseCase>()
            .AddScoped<IRecuperarReceitaPorIdUseCase, RecuperarReceitaPorIdUseCase>()
            .AddScoped<IAtualizarReceitaUseCase, AtualizarReceitaUseCase>();
    }
}