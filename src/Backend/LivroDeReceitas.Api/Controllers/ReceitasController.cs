using LivroDeReceitas.Api.Binder;
using LivroDeReceitas.Api.Filtros;
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
}