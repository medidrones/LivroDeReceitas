using LivroDeReceitas.Domain.Entidades;
using LivroDeReceitas.Domain.Repositorios.Usuario;
using Moq;

namespace Utilitario.Test.Repositorios;

public class UsuarioUpdateOnlyRepositorioBuilder
{
    private static UsuarioUpdateOnlyRepositorioBuilder _instance;
    private readonly Mock<IUsuarioUpdateOnlyRepositorio> _repositorio;

    public UsuarioUpdateOnlyRepositorioBuilder()
    {
        if (_repositorio is null)
        {
            _repositorio = new Mock<IUsuarioUpdateOnlyRepositorio>();
        }
    }

    public static UsuarioUpdateOnlyRepositorioBuilder Instancia()
    {
        _instance = new UsuarioUpdateOnlyRepositorioBuilder();

        return _instance;
    }

    public UsuarioUpdateOnlyRepositorioBuilder RecuperarPorId(Usuario usuario)
    {
        _repositorio.Setup(c => c.RecuperarPorId(usuario.Id)).ReturnsAsync(usuario);

        return this;
    }

    public IUsuarioUpdateOnlyRepositorio Construir()
    {
        return _repositorio.Object;
    }
}