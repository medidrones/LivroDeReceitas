using FluentAssertions;
using LivroDeReceitas.Application.UseCases.Receita.RecuperarPorId;
using LivroDeReceitas.Comunicacao.Enum;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using LivroDeReceitas.Exceptions;
using Utilitario.Test.Entidades;
using Utilitario.Test.Mapper;
using Utilitario.Test.Repositorios;
using Utilitario.Test.UsuarioLogado;
using Xunit;

namespace UseCases.Test.Receita.RecuperarPorId;

public class RecuperarReceitaPorIdUseCaseTest
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var receita = ReceitaBuilder.Construir(usuario);
        var useCase = CriarUseCase(usuario, receita);
        var resposta = await useCase.Executar(receita.Id);

        resposta.Titulo.Should().Be(receita.Titulo);
        resposta.Categoria.Should().Be((Categoria)receita.Categoria);
        resposta.ModoPreparo.Should().Be(receita.ModoPreparo);
        resposta.TempoPreparo.Should().Be(receita.TempoPreparo);
        resposta.Ingredientes.Should().HaveCount(receita.Ingredientes.Count);
    }

    [Fact]
    public async Task Validar_Erro_Receita_Nao_Existe()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var receita = ReceitaBuilder.Construir(usuario);

        var useCase = CriarUseCase(usuario, receita);

        Func<Task> acao = async () => { await useCase.Executar(0); };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(exception => exception.MensagensDeErro.Count == 1 && exception.MensagensDeErro.Contains(ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA));
    }

    [Fact]
    public async Task Validar_Erro_Receita_Nao_Pertence_Usuario_Logado()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();
        (var usuario2, _) = UsuarioBuilder.ConstruirUsuario2();

        var receita = ReceitaBuilder.Construir(usuario2);

        var useCase = CriarUseCase(usuario, receita);

        Func<Task> acao = async () => { await useCase.Executar(receita.Id); };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(exception => exception.MensagensDeErro.Count == 1 && exception.MensagensDeErro.Contains(ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA));
    }

    private static RecuperarReceitaPorIdUseCase CriarUseCase(LivroDeReceitas.Domain.Entidades.Usuario usuario,
        LivroDeReceitas.Domain.Entidades.Receita receita)
    {
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
        var mapper = MapperBuilder.Instancia();
        var repositorioRead = ReceitaReadOnlyRepositorioBuilder.Instancia().RecuperarPorId(receita).Construir();

        return new RecuperarReceitaPorIdUseCase(repositorioRead, usuarioLogado, mapper);
    }
}