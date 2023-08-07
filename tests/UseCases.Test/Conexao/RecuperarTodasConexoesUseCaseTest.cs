using FluentAssertions;
using LivroDeReceitas.Application.UseCases.Conexao.Recuperar;
using Utilitario.Test.Entidades;
using Utilitario.Test.Mapper;
using Utilitario.Test.Repositorios;
using Utilitario.Test.UsuarioLogado;
using Xunit;

namespace UseCases.Test.Conexao;

public class RecuperarTodasConexoesUseCaseTest
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public async Task Validar_Sucesso(int quantidadeReceitas)
    {
        (var usuario, var _) = UsuarioBuilder.Construir();
        var conexoes = ConexaoBuilder.Construir();

        var useCase = CriarUseCase(usuario, conexoes, quantidadeReceitas);

        var resultado = await useCase.Executar();

        resultado.Should().NotBeNull();
        resultado.Usuarios.Should().NotBeEmpty();
        resultado.Usuarios.Should().HaveCount(conexoes.Count);
        resultado.Usuarios.Should().SatisfyRespectively
        (
            primeiro =>
            {
                primeiro.Id.Should().NotBeNullOrWhiteSpace();
                primeiro.Nome.Should().Be(conexoes.First().Nome);
                primeiro.QuantidadeReceitas.Should().Be(quantidadeReceitas);
            }
        );
    }

    private static RecuperarTodasConexoesUseCase CriarUseCase(
        LivroDeReceitas.Domain.Entidades.Usuario usuario,
        IList<LivroDeReceitas.Domain.Entidades.Usuario> conexoes,
        int quantidadeReceitas)
    {
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
        var repositorioReceita = ReceitaReadOnlyRepositorioBuilder.Instancia().QuantidadeReceitas(quantidadeReceitas).Construir();
        var repositorioConexao = ConexaoReadOnlyRepositorioBuilder.Instancia().RecuperarDoUsuario(usuario, conexoes).Construir();
        var automapper = MapperBuilder.Instancia();

        return new RecuperarTodasConexoesUseCase(usuarioLogado, repositorioConexao, automapper, repositorioReceita);
    }
}