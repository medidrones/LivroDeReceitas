namespace LivroDeReceitas.Domain.Repositorios.Receita;

public interface IReceitaReadOnlyRepositorio
{
    Task<IList<Entidades.Receita>> RecuperarTodasDoUsuario(long usuarioId);
}