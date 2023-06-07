using FluentValidation;
using LivroDeReceitas.Comunicacao.Requisicoes;

namespace LivroDeReceitas.Application.UseCases.Receita.Atualizar;

public class AtualizarReceitaValidator : AbstractValidator<RequisicaoReceitaJson>
{
    public AtualizarReceitaValidator()
    {
        RuleFor(x => x).SetValidator(new ReceitaValidator());
    }
}