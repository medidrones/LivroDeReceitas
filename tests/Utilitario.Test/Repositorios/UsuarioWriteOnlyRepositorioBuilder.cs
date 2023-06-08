using LivroDeReceitas.Domain.Repositorios.Usuario;
using Moq;

namespace Utilitario.Test.Repositorios;

public class UsuarioWriteOnlyRepositorioBuilder
{
    private static UsuarioWriteOnlyRepositorioBuilder _instance;
    private readonly Mock<IUsuarioWriteOnlyRepositorio> _repositorio;

    public UsuarioWriteOnlyRepositorioBuilder()
    {
        if (_repositorio is null)
        {
            _repositorio = new Mock<IUsuarioWriteOnlyRepositorio>();
        }
    }

    public static UsuarioWriteOnlyRepositorioBuilder Instancia()
    {
        _instance = new UsuarioWriteOnlyRepositorioBuilder();

        return _instance;
    }

    public IUsuarioWriteOnlyRepositorio Construir()
    {
        return _repositorio.Object;
    }
}