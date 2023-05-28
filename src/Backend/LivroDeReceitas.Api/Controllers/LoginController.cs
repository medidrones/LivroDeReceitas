using LivroDeReceitas.Application.UseCases.Usuario.Login.FazerLogin;
using LivroDeReceitas.Comunicacao.Requisicoes;
using LivroDeReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace LivroDeReceitas.Api.Controllers;

public class LoginController : LivroDeReceitasController
{
    [HttpPost]
    [ProducesResponseType(typeof(RespostaLoginJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(
        [FromServices] ILoginUseCase useCase,
        [FromBody] RequisicaoLoginJson requisicao)
    {
        var resposta = await useCase.Executar(requisicao);

        return Ok(resposta);
    }
}