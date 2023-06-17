using FluentAssertions;
using LivroDeReceitas.Application.UseCases.Conexao.RecusarConexao;
using Utilitario.Test.Entidades;
using Utilitario.Test.Hashids;
using Utilitario.Test.Repositorios;
using Utilitario.Test.UsuarioLogado;
using Xunit;

namespace UseCases.Test.Conexao;

public class RecusarConexaoUseCaseTest
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        var hashids = HashidsBuilder.Instance().Build();

        (var usuario, var _) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        var resultado = await useCase.Executar();

        resultado.Should().NotBeNullOrWhiteSpace();
        resultado.Should().Be(hashids.EncodeLong(usuario.Id));
    }

    private static RecusarConexaoUseCase CriarUseCase(LivroDeReceitas.Domain.Entidades.Usuario usuario)
    {
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
        var repositorioWrite = CodigoWriteOnlyRepositorioBuilder.Instancia().Construir();
        var unidadeDeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();
        var hashids = HashidsBuilder.Instance().Build();

        return new RecusarConexaoUseCase(repositorioWrite, usuarioLogado, unidadeDeTrabalho, hashids);
    }
}