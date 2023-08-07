using LivroDeReceitas.Comunicacao.Requisicoes;
using System.Net;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace WebApi.Test.V1.Dashboard;

public class DashboardTest : ControllerBase
{
    private const string METODO = "dashboard";

    private LivroDeReceitas.Domain.Entidades.Usuario _usuarioComReceita;
    private string _senhaUsuarioComReceita;

    private LivroDeReceitas.Domain.Entidades.Usuario _usuarioSemReceita;
    private string _senhaUsuarioSemReceita;

    public DashboardTest(WebApiFactory<Program> factory) : base(factory)
    {
        _usuarioComReceita = factory.RecuperarUsuario();
        _senhaUsuarioComReceita = factory.RecuperarSenha();

        _usuarioSemReceita = factory.RecuperarUsuarioSemReceita();
        _senhaUsuarioSemReceita = factory.RecuperarSenhaUsuarioSemReceita();
    }

    [Fact]
    public async Task Validar_Sucesso()
    {
        var token = await Login(_usuarioComReceita.Email, _senhaUsuarioComReceita);

        var resposta = await PutRequest(METODO, new RequisicaoDashboardJson(), token);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responstaBody);

        responseData.RootElement.GetProperty("receitas").GetArrayLength().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Validar_Erro_Receita_Inexistente()
    {
        var token = await Login(_usuarioSemReceita.Email, _senhaUsuarioSemReceita);

        var resposta = await PutRequest(METODO, new RequisicaoDashboardJson(), token);

        resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}