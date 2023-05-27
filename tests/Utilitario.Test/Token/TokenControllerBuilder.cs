using LivroDeReceitas.Application.Servicos.Token;

namespace Utilitario.Test.Token;

public class TokenControllerBuilder
{
    public static TokenController Instancia()
    {
        return new TokenController(1000, "eHFDZjRrZkJxZ05YVzhzMEVhTkpHT3UyKmIhQGtO");
    }
}