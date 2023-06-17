namespace LivroDeReceitas.Domain.Repositorios.Codigo;

public interface ICodigoReadOnlyRepositorio
{
    Task<Entidades.Codigos> RecuperarEntidadeCodigo(string codigo);
}