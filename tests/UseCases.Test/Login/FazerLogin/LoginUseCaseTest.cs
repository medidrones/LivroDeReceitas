using FluentAssertions;
using LivroDeReceitas.Application.UseCases.Usuario.Login.FazerLogin;
using LivroDeReceitas.Comunicacao.Requisicoes;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using Utilitario.Test.Criptografia;
using Utilitario.Test.Entidades;
using Utilitario.Test.Repositorios;
using Utilitario.Test.Token;
using Xunit;

namespace UseCases.Test.Login.FazerLogin;

public class LoginUseCaseTest
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        var resposta = await useCase.Executar(new RequisicaoLoginJson
        {
            Email = usuario.Email,
            Senha = senha
        });

        resposta.Should().NotBeNull();
        resposta.Nome.Should().Be(usuario.Nome);
        resposta.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Validar_Erro_Senha_Invalida()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        Func<Task> acao = async () =>
        {
            await useCase.Executar(new RequisicaoLoginJson
            {
                Email = usuario.Email,
                Senha = "senhaInvalida"
            });
        };

        await acao.Should().ThrowAsync<LoginInvalidoException>()
            .Where(exception => exception.Message.Equals(ResourceMensagensDeErro.LOGIN_INVALIDO));
    }

    [Fact]
    public async Task Validar_Erro_Email_Invalida()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        Func<Task> acao = async () =>
        {
            await useCase.Executar(new RequisicaoLoginJson
            {
                Email = "email@invalido.com",
                Senha = senha
            });
        };

        await acao.Should().ThrowAsync<LoginInvalidoException>()
            .Where(exception => exception.Message.Equals(ResourceMensagensDeErro.LOGIN_INVALIDO));
    }

    [Fact]
    public async Task Validar_Erro_Email_E_Senha_Invalida()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        Func<Task> acao = async () =>
        {
            await useCase.Executar(new RequisicaoLoginJson
            {
                Email = "email@invalido.com",
                Senha = "senhaInvalida"
            });
        };

        await acao.Should().ThrowAsync<LoginInvalidoException>()
            .Where(exception => exception.Message.Equals(ResourceMensagensDeErro.LOGIN_INVALIDO));
    }

    private static LoginUseCase CriarUseCase(LivroDeReceitas.Domain.Entidades.Usuario usuario)
    {
        var encriptador = EncriptadorDeSenhaBuilder.Instancia();
        var token = TokenControllerBuilder.Instancia();
        var repositorioReadOnly = UsuarioReadOnlyRepositorioBuilder.Instancia().RecuperarPorEmailSenha(usuario).Construir();

        return new LoginUseCase(repositorioReadOnly, encriptador, token);
    }
}