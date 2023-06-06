using LivroDeReceitas.Comunicacao.Requisicoes;
using LivroDeReceitas.Comunicacao.Respostas;

namespace LivroDeReceitas.Application.UseCases.Dashboard;

public interface IDashboardUseCase
{
    Task<RespostaDashboardJson> Executar(RequisicaoDashboardJson requisicao);
}