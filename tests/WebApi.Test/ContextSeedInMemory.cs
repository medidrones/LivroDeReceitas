﻿using LivroDeReceitas.Domain.Entidades;
using LivroDeReceitas.Infrastructure.AcessoRepositorio;
using Utilitario.Test.Entidades;

namespace WebApi.Test;

public class ContextSeedInMemory
{
    public static (Usuario usuario, string senha) Seed(LivroDeReceitasContext context)
    {
        (var usuario, string senha) = UsuarioBuilder.Construir();
        var receita = ReceitaBuilder.Construir(usuario);

        context.Usuarios.Add(usuario);
        context.Receitas.Add(receita);

        context.SaveChanges();

        return (usuario, senha);
    }

    public static (Usuario usuario, string senha) SeedUsuarioSemReceita(LivroDeReceitasContext context)
    {
        (var usuario, string senha) = UsuarioBuilder.ConstruirUsuario2();

        context.Usuarios.Add(usuario);

        context.SaveChanges();

        return (usuario, senha);
    }

    public static (Usuario usuario, string senha) SeedUsuarioComConexao(LivroDeReceitasContext context)
    {
        (var usuario, string senha) = UsuarioBuilder.ConstruirUsuarioComConexao();

        context.Usuarios.Add(usuario);

        var usuarioConexoes = ConexaoBuilder.Construir();

        for (var index = 1; index <= usuarioConexoes.Count; index++)
        {
            var conexaoComUsuario = usuarioConexoes[index - 1];

            context.Conexoes.Add(new Conexao
            {
                Id = index,
                UsuarioId = usuario.Id,
                ConectadoComUsuario = conexaoComUsuario
            });
        }

        context.SaveChanges();

        return (usuario, senha);
    }
}