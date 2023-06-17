using FluentAssertions;
using LivroDeReceitas.Application.UseCases.Conexao.Remover;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using LivroDeReceitas.Exceptions;
using UseCases.Test.Conexao.InlineData;
using Utilitario.Test.Entidades;
using Utilitario.Test.Repositorios;
using Utilitario.Test.UsuarioLogado;
using Xunit;

namespace UseCases.Test.Conexao;

public class RemoverConexaoUseCaseTest
{
    [Theory]
    [ClassData(typeof(EntidadesUsuarioConexaoDataTest))]
    public async Task Validar_Sucesso(long usuarioIdParaRemover, IList<LivroDeReceitas.Domain.Entidades.Usuario> conexoes)
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario, conexoes);

        Func<Task> acao = async () => { await useCase.Executar(usuarioIdParaRemover); };

        await acao.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Validar_Erro_UsuarioInvalido()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var conexoes = ConexaoBuilder.Construir();

        var useCase = CriarUseCase(usuario, conexoes);

        Func<Task> acao = async () => { await useCase.Executar(0); };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(exception => exception.MensagensDeErro.Count == 1 && exception.MensagensDeErro.Contains(ResourceMensagensDeErro.USUARIO_NAO_ENCONTRADO));
    }

    private static RemoverConexaoUseCase CriarUseCase(
        LivroDeReceitas.Domain.Entidades.Usuario usuario,
        IList<LivroDeReceitas.Domain.Entidades.Usuario> conexoes)
    {
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
        var repositorioReadOnly = ConexaoReadOnlyRepositorioBuilder.Instancia().RecuperarDoUsuario(usuario, conexoes).Construir();
        var repositorioWriteOnly = ConexaoWriteOnlyRepositorioBuilder.Instancia().Construir();
        var unidadeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();

        return new RemoverConexaoUseCase(usuarioLogado, repositorioReadOnly, repositorioWriteOnly, unidadeTrabalho);
    }
}