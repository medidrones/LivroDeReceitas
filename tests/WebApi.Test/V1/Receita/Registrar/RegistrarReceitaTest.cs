using System.Net;
using System.Text.Json;
using FluentAssertions;
using Utilitario.Test.Requisicoes;
using Xunit;

namespace WebApi.Test.V1.Receita.Registrar;

public class RegistrarReceitaTest : ControllerBase
{
    private const string METODO = "receitas";
    private LivroDeReceitas.Domain.Entidades.Usuario _usuario;
    private string _senha;

    public RegistrarReceitaTest(WebApiFactory<Program> factory) : base(factory)
    {
        _usuario = factory.RecuperarUsuario();
        _senha = factory.RecuperarSenha();
    }

    [Fact]
    public async Task Validar_Sucesso()
    {
        var token = await Login(_usuario.Email, _senha);
        var requisicao = RequisicaoReceitaBuilder.Construir();
        var resposta = await PostRequest(METODO, requisicao, token);

        resposta.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var respostaBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(respostaBody);

        responseData.RootElement.GetProperty("id").GetString().Should().NotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("titulo").GetString().Should().Be(requisicao.Titulo);
        responseData.RootElement.GetProperty("categoria").GetUInt16().Should().Be((ushort)requisicao.Categoria);
        responseData.RootElement.GetProperty("modoPreparo").GetString().Should().Be(requisicao.ModoPreparo);
        //responseData.RootElement.GetProperty("tempoPreparo").GetInt32().Should().Be(requisicao.TempoPreparo);
    }

    /*[Fact]
    public async Task Validar_Erro_Sem_Ingredientes()
    {
        var token = await Login(_usuario.Email, _senha);
        var requisicao = RequisicaoReceitaBuilder.Construir();
        requisicao.Ingredientes.Clear();

        var resposta = await PostRequest(METODO, requisicao, token);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responstaBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responstaBody);

        var erros = responseData.RootElement.GetProperty("mensagens").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(c => c.GetString().Equals(ResourceMensagensDeErro.NOME_USUARIO_EM_BRANCO));
    }*/
}