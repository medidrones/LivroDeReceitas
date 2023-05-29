using LivroDeReceitas.Domain.Entidades;
using LivroDeReceitas.Domain.Repositorios.Usuario;
using Microsoft.EntityFrameworkCore;

namespace LivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;

public class UsuarioRepositorio : IUsuarioWriteOnlyRepositorio, IUsuarioReadOnlyRepositorio, IUpdateOnlyRepositorio
{
    private readonly LivroDeReceitasContext _contexto;

    public UsuarioRepositorio(LivroDeReceitasContext contexto)
    {
        _contexto = contexto;
    }

    public async Task Adicionar(Usuario usuario)
    {
        await _contexto.Usuarios.AddAsync(usuario);
    }

    public async Task<bool> ExisteUsuarioComEmail(string email)
    {
        return await _contexto.Usuarios.AnyAsync(c => c.Email.Equals(email));
    }

    public async Task<Usuario> RecuperarPorEmailSenha(string email, string senha)
    {
        return await _contexto.Usuarios
            .FirstOrDefaultAsync(c => c.Email.Equals(email) && c.Senha.Equals(senha));
    }

    public void Update(Usuario usuario)
    {
        _contexto.Usuarios.Update(usuario);
    }
}