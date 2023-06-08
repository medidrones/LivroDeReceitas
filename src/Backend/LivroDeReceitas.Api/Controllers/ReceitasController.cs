using LivroDeReceitas.Api.Binder;
using LivroDeReceitas.Api.Filtros;
using LivroDeReceitas.Application.UseCases.Receita.Atualizar;
using LivroDeReceitas.Application.UseCases.Receita.Deletar;
using LivroDeReceitas.Application.UseCases.Receita.RecuperarPorId;
using LivroDeReceitas.Application.UseCases.Receita.Registrar;
using LivroDeReceitas.Comunicacao.Requisicoes;
using LivroDeReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace LivroDeReceitas.Api.Controllers;

[ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
public class ReceitasController : LivroDeReceitasController
{
    [HttpPost]
    [ProducesResponseType(typeof(RespostaReceitaJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Registrar(
        [FromServices] IRegistrarReceitaUseCase useCase,
        [FromBody] RequisicaoReceitaJson requisicao)
        
    {
        var resposta = await useCase.Executar(requisicao);

        return Created(string.Empty, resposta);
    }

    [HttpGet]
    [Route("{id:hashids}")]
    [ProducesResponseType(typeof(RespostaReceitaJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecuperarPorId(
        [FromServices] IRecuperarReceitaPorIdUseCase useCase,
        [FromRoute][ModelBinder(typeof(HashidsModelBinder))] long id)
    {
        var resposta = await useCase.Executar(id);

        return Ok(resposta);
    }

    [HttpPut]
    [Route("{id:hashids}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Atualizar(
        [FromServices] IAtualizarReceitaUseCase useCase,
        [FromBody] RequisicaoReceitaJson requisicao,
        [FromRoute][ModelBinder(typeof(HashidsModelBinder))] long id)
    {
        await useCase.Executar(id, requisicao);

        return NoContent();
    }

    [HttpDelete]
    [Route("{id:hashids}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Deletar(
        [FromServices] IDeletarReceitaUseCase useCase,
        [FromRoute][ModelBinder(typeof(HashidsModelBinder))] long id)
    {
        await useCase.Executar(id);

        return NoContent();
    }
}