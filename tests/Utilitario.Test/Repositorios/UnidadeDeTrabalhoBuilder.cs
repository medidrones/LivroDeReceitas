using LivroDeReceitas.Domain.Repositorios;
using Moq;

namespace Utilitario.Test.Repositorios;

public class UnidadeDeTrabalhoBuilder
{
    private static UnidadeDeTrabalhoBuilder _instance;
    private readonly Mock<IUnidadeDeTrabalho> _repositorio;

    public UnidadeDeTrabalhoBuilder()
    {
        if (_repositorio == null)
        {
            _repositorio = new Mock<IUnidadeDeTrabalho>();
        }
    }

    public static UnidadeDeTrabalhoBuilder Instancia()
    {
        _instance = new UnidadeDeTrabalhoBuilder();

        return _instance;
    }

    public IUnidadeDeTrabalho Construir()
    {
        return _repositorio.Object;
    }
}