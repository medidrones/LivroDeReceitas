using AspNetCore.Hashids.Mvc;
using LivroDeReceitas.Api.Filtros.UsuarioLogado;
using LivroDeReceitas.Application.UseCases.Conexao.Recuperar;
using LivroDeReceitas.Application.UseCases.Conexao.Remover;
using LivroDeReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace LivroDeReceitas.Api.Controllers;

[ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
public class ConexoesController : LivroDeReceitasController
{
    [HttpGet]
    [ProducesResponseType(typeof(RespostaConexoesDoUsuarioJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RecuperarConexoes([FromServices] IRecuperarTodasConexoesUseCase useCase)
    {
        var resultado = await useCase.Executar();

        if (resultado.Usuarios.Any())
        {
            return Ok(resultado);
        }

        return NoContent();
    }

    [HttpDelete]
    [Route("{id:hashids}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Deletar(
        [FromServices] IRemoverConexaoUseCase useCase,
        [FromRoute][ModelBinder(typeof(HashidsModelBinder))] long id)
    {
        await useCase.Executar(id);

        return NoContent();
    }
}