using FluentAssertions;
using LivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;
using LivroDeReceitas.Comunicacao.Requisicoes;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using LivroDeReceitas.Exceptions;
using Utilitario.Test.Criptografia;
using Utilitario.Test.Entidades;
using Utilitario.Test.Repositorios;
using Utilitario.Test.UsuarioLogado;
using Xunit;
using Utilitario.Test.Requisicoes;

namespace UseCases.Test.Usuario.AlterarSenha;

public class AlterarSenhaUseCaseTest
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        Func<Task> acao = async () =>
        {
            await useCase.Executar(new RequisicaoAlterarSenhaJson()
            {
                SenhaAtual = senha,
                NovaSenha = "@NovaSenha1234"
            });
        };

        await acao.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Validar_Erro_NovaSenha_Em_Branco()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        Func<Task> acao = async () =>
        {
            await useCase.Executar(new RequisicaoAlterarSenhaJson
            {
                SenhaAtual = senha,
                NovaSenha = ""
            });
        };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(ex => ex.MensagensDeErro.Count == 1 && ex.MensagensDeErro.Contains(ResourceMensagensDeErro.SENHA_USUARIO_EM_BRANCO));
    }

    [Fact]
    public async Task Validar_Erro_SenhaAtual_Invalida()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();
        requisicao.SenhaAtual = "senhaInvalida";

        Func<Task> acao = async () =>
        {
            await useCase.Executar(requisicao);
        };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(ex => ex.MensagensDeErro.Count == 1 && ex.MensagensDeErro.Contains(ResourceMensagensDeErro.SENHA_ATUAL_INVALIDA));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Validar_Erro_SenhaAtual_Minimo_Caracteres(int tamanhoSenha)
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir(tamanhoSenha);
        requisicao.SenhaAtual = senha;

        Func<Task> acao = async () =>
        {
            await useCase.Executar(requisicao);
        };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(ex => ex.MensagensDeErro.Count == 1 && ex.MensagensDeErro.Contains(ResourceMensagensDeErro.SENHA_USUARIO_MINIMO_SEIS_CARACTERES));
    }

    private static AlterarSenhaUseCase CriarUseCase(LivroDeReceitas.Domain.Entidades.Usuario usuario)
    {
        var encriptador = EncriptadorDeSenhaBuilder.Instancia();
        var unidadeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();
        var repositorio = UsuarioUpdateOnlyRepositorioBuilder.Instancia().RecuperarPorId(usuario).Construir();
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();

        return new AlterarSenhaUseCase(repositorio, usuarioLogado, encriptador, unidadeTrabalho);
    }
}