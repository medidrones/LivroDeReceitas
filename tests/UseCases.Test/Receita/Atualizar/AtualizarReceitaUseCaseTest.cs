﻿using FluentAssertions;
using LivroDeReceitas.Application.UseCases.Receita.Atualizar;
using LivroDeReceitas.Domain.Enum;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using LivroDeReceitas.Exceptions;
using Utilitario.Test.Entidades;
using Utilitario.Test.Mapper;
using Utilitario.Test.Repositorios;
using Utilitario.Test.Requisicoes;
using Utilitario.Test.UsuarioLogado;
using Xunit;

namespace UseCases.Test.Receita.Atualizar;

public class AtualizarReceitaUseCaseTest
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var receita = ReceitaBuilder.Construir(usuario);

        var useCase = CriarUseCase(usuario, receita);

        var requisicao = RequisicaoReceitaBuilder.Construir();

        await useCase.Executar(receita.Id, requisicao);

        Func<Task> acao = async () => { await useCase.Executar(usuario.Id, requisicao); };

        await acao.Should().NotThrowAsync();

        receita.Titulo.Should().Be(requisicao.Titulo);
        receita.Categoria.Should().Be((Categoria)requisicao.Categoria);
        receita.ModoPreparo.Should().Be(requisicao.ModoPreparo);
        receita.TempoPreparo.Should().Be(requisicao.TempoPreparo);
        receita.Ingredientes.Should().HaveCount(requisicao.Ingredientes.Count);
    }

    [Fact]
    public async Task Validar_Erro_Ingredientes_Vazio()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();

        var receita = ReceitaBuilder.Construir(usuario);

        var useCase = CriarUseCase(usuario, receita);

        var requisicao = RequisicaoReceitaBuilder.Construir();
        requisicao.Ingredientes.Clear();

        Func<Task> acao = async () => { await useCase.Executar(receita.Id, requisicao); };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(exception => exception.MensagensDeErro.Count == 1 &&
                                exception.MensagensDeErro.Contains(
                                    ResourceMensagensDeErro.RECEITA_MINIMO_UM_INGREDIENTE));
    }

    [Fact]
    public async Task Validar_Erro_Receita_Nao_Existe()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();

        var receita = ReceitaBuilder.Construir(usuario);

        var useCase = CriarUseCase(usuario, receita);

        var requisicao = RequisicaoReceitaBuilder.Construir();

        Func<Task> acao = async () => { await useCase.Executar(0, requisicao); };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(exception => exception.MensagensDeErro.Count == 1 &&
                                exception.MensagensDeErro.Contains(ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA));
    }

    [Fact]
    public async Task Validar_Erro_Receita_Nao_Pertence_Usuario_Logado()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();
        (var usuario2, _) = UsuarioBuilder.ConstruirUsuario2();

        var receita = ReceitaBuilder.Construir(usuario2);

        var useCase = CriarUseCase(usuario, receita);

        var requisicao = RequisicaoReceitaBuilder.Construir();

        Func<Task> acao = async () => { await useCase.Executar(receita.Id, requisicao); };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(exception => exception.MensagensDeErro.Count == 1 &&
                                exception.MensagensDeErro.Contains(ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA));
    }

    private static AtualizarReceitaUseCase CriarUseCase(LivroDeReceitas.Domain.Entidades.Usuario usuario,
        LivroDeReceitas.Domain.Entidades.Receita receita)
    {
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
        var mapper = MapperBuilder.Instancia();
        var repositorio = ReceitaUpdateOnlyRepositorioBuilder.Instancia().RecuperarPorId(receita).Construir();
        var unidadeDeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();

        return new AtualizarReceitaUseCase(repositorio, usuarioLogado, mapper, unidadeDeTrabalho);
    }
}