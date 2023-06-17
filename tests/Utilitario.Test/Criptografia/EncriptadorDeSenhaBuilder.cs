using LivroDeReceitas.Application.Servicos.Criptografia;

namespace Utilitario.Test.Criptografia;

public class EncriptadorDeSenhaBuilder
{
    public static EncriptadorDeSenha Instancia()
    {
        return new EncriptadorDeSenha("ABCD123");
    }
}