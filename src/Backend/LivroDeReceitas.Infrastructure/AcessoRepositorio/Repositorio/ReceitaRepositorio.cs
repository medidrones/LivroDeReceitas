using LivroDeReceitas.Domain.Entidades;
using LivroDeReceitas.Domain.Repositorios.Receita;
using Microsoft.EntityFrameworkCore;

namespace LivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;

public class ReceitaRepositorio : IReceitaWriteOnlyRepositorio, IReceitaReadOnlyRepositorio
{
    private readonly LivroDeReceitasContext _contexto;

    public ReceitaRepositorio(LivroDeReceitasContext contexto)
    {
        _contexto = contexto;
    }

    public async Task Registrar(Receita receita)
    {
        await _contexto.Receitas.AddAsync(receita);
    }

    public async Task<IList<Receita>> RecuperarTodasDoUsuario(long usuarioId)
    {
        return await _contexto.Receitas.Where(r => r.UsuarioId == usuarioId).ToListAsync();
    }
}