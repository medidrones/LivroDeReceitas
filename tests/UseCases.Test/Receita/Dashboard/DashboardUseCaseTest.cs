using FluentAssertions;
using LivroDeReceitas.Application.UseCases.Dashboard;
using LivroDeReceitas.Comunicacao.Enum;
using LivroDeReceitas.Comunicacao.Requisicoes;
using Utilitario.Test.Entidades;
using Utilitario.Test.Mapper;
using Utilitario.Test.Repositorios;
using Utilitario.Test.UsuarioLogado;
using Xunit;

namespace UseCases.Test.Receita.Dashboard;

public class DashboardUseCaseTest
{
    [Fact]
    public async Task Validar_Sucesso_Sem_Receitas()
    {
        (var usuario, var _) = UsuarioBuilder.ConstruirUsuario2();

        var conexoes = ConexaoBuilder.Construir();
        var useCase = CriarUseCase(usuario, conexoes);
        var resposta = await useCase.Executar(new());

        resposta.Receitas.Should().HaveCount(0);
    }

    [Fact]
    public async Task Validar_Sucesso_Sem_Filtro()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var conexoes = ConexaoBuilder.Construir();
        var receita = ReceitaBuilder.Construir(usuario);
        var useCase = CriarUseCase(usuario, conexoes, receita);
        var resposta = await useCase.Executar(new());

        resposta.Receitas.Should().HaveCountGreaterThan(0);
    }

    /*[Fact]
    public async Task Validar_Sucesso_Filtro_Titulo()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var conexoes = ConexaoBuilder.Construir();
        var receita = ReceitaBuilder.Construir(usuario);
        var useCase = CriarUseCase(usuario, conexoes, receita);
        var resposta = await useCase.Executar(new RequisicaoDashboardJson
        {
            TituloOuIngrediente = receita.Titulo.ToUpper()
        });

        resposta.Receitas.Should().HaveCountGreaterThan(0);
    }*/

    /*[Fact]
    public async Task Validar_Sucesso_Filtro_Ingredientes()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var conexoes = ConexaoBuilder.Construir();
        var receita = ReceitaBuilder.Construir(usuario);
        var useCase = CriarUseCase(usuario, conexoes, receita);
        var resposta = await useCase.Executar(new RequisicaoDashboardJson
        {
            TituloOuIngrediente = receita.Ingredientes.First().Produto.ToUpper()
        });

        resposta.Receitas.Should().HaveCountGreaterThan(0);
    }*/

    [Fact]
    public async Task Validar_Sucesso_Filtro_Categoria()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var conexoes = ConexaoBuilder.Construir();
        var receita = ReceitaBuilder.Construir(usuario);
        var useCase = CriarUseCase(usuario, conexoes, receita);
        var resposta = await useCase.Executar(new RequisicaoDashboardJson
        {
            Categoria = (Categoria)receita.Categoria
        });

        resposta.Receitas.Should().HaveCountGreaterThan(0);
    }

    private static DashboardUseCase CriarUseCase(
        LivroDeReceitas.Domain.Entidades.Usuario usuario,
        IList<LivroDeReceitas.Domain.Entidades.Usuario> usuariosConectados,
        LivroDeReceitas.Domain.Entidades.Receita? receita = null)
    {
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
        var mapper = MapperBuilder.Instancia();
        var repositorioRead = ReceitaReadOnlyRepositorioBuilder.Instancia().RecuperarTodasDoUsuario(receita).Construir();
        //var repositorioConexao = ConexaoReadOnlyRepositorioBuilder.Instancia().RecuperarDoUsuario(usuario, usuariosConectados).Construir();

        return new DashboardUseCase(repositorioRead, usuarioLogado, mapper);
    }
}