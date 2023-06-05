using FluentAssertions;
using LivroDeReceitas.Application.UseCases.Receita.Registrar;
using LivroDeReceitas.Exceptions;
using LivroDeReceitas.Exceptions.ExceptionsBase;
using Utilitario.Test.Entidades;
using Utilitario.Test.Mapper;
using Utilitario.Test.Repositorios;
using Utilitario.Test.Requisicoes;
using Utilitario.Test.UsuarioLogado;
using Xunit;

namespace UseCases.Test.Receita.Registrar;

public class RegistrarReceitaUseCaseTest
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        (var usuario, var _) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);
        var requicao = RequisicaoReceitaBuilder.Construir();
        var resposta = await useCase.Executar(requicao);
    }

    [Fact]
    public async Task Validar_Erro_Email_Vazio()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        var requisicao = RequisicaoReceitaBuilder.Construir();
        requisicao.Ingredientes.Clear();

        Func<Task> acao = async () => { await useCase.Executar(requisicao); };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(exception => exception.MensagensDeErro.Count == 1 &&
                                exception.MensagensDeErro.Contains(
                                    ResourceMensagensDeErro.RECEITA_MINIMO_UM_INGREDIENTE));
    }

    private static RegistrarReceitaUseCase CriarUseCase(LivroDeReceitas.Domain.Entidades.Usuario usuario)
    {
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
        var mapper = MapperBuilder.Instancia();
        var repositorio = ReceitaWriteOnlyRepositorioBuilder.Instancia().Construir();
        var unidadeDeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();

        return new RegistrarReceitaUseCase(mapper, unidadeDeTrabalho, usuarioLogado, repositorio);
    }
}