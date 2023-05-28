using LivroDeReceitas.Domain.Entidades;

namespace LivroDeReceitas.Domain.Repositorios;

public interface IUsuarioReadOnlyRepositorio
{
    Task<bool> ExisteUsuarioComEmail(string email);
    Task<Usuario> Login(string email, string senha);
}