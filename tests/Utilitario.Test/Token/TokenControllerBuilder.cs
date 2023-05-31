using LivroDeReceitas.Application.Servicos.Token;

namespace Utilitario.Test.Token;

public class TokenControllerBuilder
{
    public static TokenController Instancia()
    {
        return new TokenController(1000, "RjFPQEBwaTQjJWZ3Vm5QSVloWWE0N2IzcmZ3M093");
    }

    public static TokenController TokenExpirado()
    {
        return new TokenController(0.0166667, "RjFPQEBwaTQjJWZ3Vm5QSVloWWE0N2IzcmZ3M093");
    }
}