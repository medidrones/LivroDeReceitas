using LivroDeReceitas.Api.Filtros;
using LivroDeReceitas.Application.UseCases.Dashboard;
using LivroDeReceitas.Comunicacao.Requisicoes;
using LivroDeReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace LivroDeReceitas.Api.Controllers;

public class DashboardController : LivroDeReceitasController
{
    [HttpPut]
    [ProducesResponseType(typeof(RespostaDashboardJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
    public async Task<IActionResult> RecuperarDashboard(
        [FromServices] IDashboardUseCase useCase,
        [FromBody] RequisicaoDashboardJson request)
    {
        var resultado = await useCase.Executar(request);

        if (resultado.Receitas.Any())
        {
            return Ok(resultado);
        }

        return NoContent();
    }
}