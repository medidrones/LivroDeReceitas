using LivroDeReceitas.Domain.Entidades;
using LivroDeReceitas.Infrastructure.AcessoRepositorio;
using Utilitario.Test.Entidades;

namespace WebApi.Test;

public class ContexSeedInMemory
{
    public static (Usuario usuario, string senha) Seed(LivroDeReceitasContext context)
    {
        (var usuario, string senha) = UsuarioBuilder.Construir();

        context.Usuarios.Add(usuario);
        context.SaveChangesAsync();

        return (usuario, senha);
    }
}