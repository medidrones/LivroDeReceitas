using FluentValidation;
using LivroDeReceitas.Comunicacao.Requisicoes;

namespace LivroDeReceitas.Application.UseCases.Receita.Registrar;

public class RegistrarReceitaValidator : AbstractValidator<RequisicaoReceitaJson>
{
    public RegistrarReceitaValidator()
    {
        RuleFor(x => x).SetValidator(new ReceitaValidator());
    }

}