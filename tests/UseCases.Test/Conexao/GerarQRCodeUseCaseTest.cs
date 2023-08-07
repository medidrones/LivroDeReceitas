using FluentAssertions;
using LivroDeReceitas.Application.UseCases.Conexao.GerarQRCode;
using Utilitario.Test.Entidades;
using Utilitario.Test.Hashids;
using Utilitario.Test.Repositorios;
using Utilitario.Test.UsuarioLogado;
using Xunit;

namespace UseCases.Test.Conexao;

public class GerarQRCodeUseCaseTest
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        var resultado = await useCase.Executar();

        resultado.Should().NotBeNull();
        resultado.qrCode.Should().NotBeEmpty();

        var hashids = HashidsBuilder.Instance().Build();
        resultado.idUsuario.Should().Be(hashids.EncodeLong(usuario.Id));
    }

    private static GerarQRCodeUseCase CriarUseCase(LivroDeReceitas.Domain.Entidades.Usuario usuario)
    {
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
        var repositorioWrite = CodigoWriteOnlyRepositorioBuilder.Instancia().Construir();
        var unidadeDeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();
        var hashids = HashidsBuilder.Instance().Build();

        return new GerarQRCodeUseCase(repositorioWrite, usuarioLogado, unidadeDeTrabalho, hashids);
    }
}