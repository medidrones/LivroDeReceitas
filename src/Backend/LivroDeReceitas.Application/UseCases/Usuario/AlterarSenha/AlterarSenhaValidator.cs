using FluentValidation;
using LivroDeReceitas.Comunicacao.Requisicoes;

namespace LivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;

public class AlterarSenhaValidator : AbstractValidator<RequisicaoAlterarSenhaJson>
{
    public AlterarSenhaValidator()
    {
        RuleFor(c => c.NovaSenha).SetValidator(new SenhaValidator());
    }
}