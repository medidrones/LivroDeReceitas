using Bogus;
using LivroDeReceitas.Comunicacao.Respostas;
using Utilitario.Test.Hashids;

namespace Utilitario.Test.Respostas;

public class RespostaUsuarioConexaoBuilder
{
    public static RespostaUsuarioConexaoJson Construir()
    {
        var hashIds = HashidsBuilder.Instance().Build();

        return new Faker<RespostaUsuarioConexaoJson>()
            .RuleFor(c => c.Id, f => hashIds.EncodeLong(f.Random.Long(1, 5000)))
            .RuleFor(c => c.Nome, f => f.Person.FullName);
    }
}