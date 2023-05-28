using LivroDeReceitas.Domain.Entidades;

namespace LivroDeReceitas.Domain.Repositorios;

public interface IUsuarioReadOnlyRepositorio
{
    Task<bool> ExisteUsuarioComEmail(string email);
    Task<Usuario> RecuperarPorEmailSenha(string email, string senha);
}