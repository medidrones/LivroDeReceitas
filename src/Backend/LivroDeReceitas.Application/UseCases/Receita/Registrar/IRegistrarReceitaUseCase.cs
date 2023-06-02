using LivroDeReceitas.Comunicacao.Requisicoes;
using LivroDeReceitas.Comunicacao.Respostas;

namespace LivroDeReceitas.Application.UseCases.Receita.Registrar;

public interface IRegistrarReceitaUseCase
{
    Task<RespostaReceitaJson> Executar(RequisicaoRegistrarReceitaJson requisicao);
}