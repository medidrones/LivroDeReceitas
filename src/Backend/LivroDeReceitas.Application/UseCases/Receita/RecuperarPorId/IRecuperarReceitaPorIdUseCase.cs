using LivroDeReceitas.Comunicacao.Respostas;

namespace LivroDeReceitas.Application.UseCases.Receita.RecuperarPorId;

public interface IRecuperarReceitaPorIdUseCase
{
    Task<RespostaReceitaJson> Executar(long id);
}