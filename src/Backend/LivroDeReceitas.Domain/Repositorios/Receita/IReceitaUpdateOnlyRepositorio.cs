namespace LivroDeReceitas.Domain.Repositorios.Receita;

public interface IReceitaUpdateOnlyRepositorio
{
    Task<Entidades.Receita> RecuperaPorId(long receitaId);
    void Update(Entidades.Receita receita);
}