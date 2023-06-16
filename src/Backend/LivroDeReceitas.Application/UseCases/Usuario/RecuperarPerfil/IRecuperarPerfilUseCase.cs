using LivroDeReceitas.Comunicacao.Respostas;

namespace LivroDeReceitas.Application.UseCases.Usuario.RecuperarPerfil;

public interface IRecuperarPerfilUseCase
{
    Task<RespostaPerfilUsuarioJson> Executar();
}