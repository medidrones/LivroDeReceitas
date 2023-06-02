using LivroDeReceitas.Domain.Entidades;
using LivroDeReceitas.Domain.Repositorios.Receita;

namespace LivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;

public class ReceitaRepositorio : IReceitaWriteOnlyRepositorio
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
}