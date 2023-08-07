using LivroDeReceitas.Comunicacao.Respostas;

namespace LivroDeReceitas.Application.UseCases.Conexao.Recuperar;

public interface IRecuperarTodasConexoesUseCase
{
    Task<RespostaConexoesDoUsuarioJson> Executar();
}