using LivroDeReceitas.Domain.Repositorios;

namespace LivroDeReceitas.Infrastructure.AcessoRepositorio;

public sealed class UnidadeDeTrabalho : IDisposable, IUnidadeDeTrabalho
{
    private readonly LivroDeReceitasContext _contexto;
    private bool _disposed;

    public UnidadeDeTrabalho(LivroDeReceitasContext contexto)
    {
        _contexto = contexto;
    }

    public async Task Commit()
    {
        await _contexto.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _contexto.Dispose();
        }

        _disposed = true;
    }
}