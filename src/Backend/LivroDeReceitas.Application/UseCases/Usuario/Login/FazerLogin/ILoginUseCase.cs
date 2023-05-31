using LivroDeReceitas.Comunicacao.Requisicoes;
using LivroDeReceitas.Comunicacao.Respostas;

namespace LivroDeReceitas.Application.UseCases.Usuario.Login.FazerLogin;

public interface ILoginUseCase
{
    Task<RespostaLoginJson> Executar(RequisicaoLoginJson request);
}