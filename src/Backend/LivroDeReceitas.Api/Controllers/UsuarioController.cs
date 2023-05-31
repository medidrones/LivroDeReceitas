using LivroDeReceitas.Api.Filtros;
using LivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;
using LivroDeReceitas.Application.UseCases.Usuario.Registrar;
using LivroDeReceitas.Comunicacao.Requisicoes;
using LivroDeReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace LivroDeReceitas.Api.Controllers
{
    public class UsuarioController : LivroDeReceitasController
    {
        [HttpPost]
        [ProducesResponseType(typeof(RespostaUsuarioRegistradoJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> RegistrarUsuario(
            [FromServices] IRegistrarUsuarioUseCase useCase,
            [FromBody] RequisicaoRegistrarUsuarioJson request)
        {
            var resultado = await useCase.Executar(request);

            return Created(string.Empty, resultado);
        }

        [HttpPut]
        [Route("alterar-senha")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
        public async Task<IActionResult> AlterarSenha(
            [FromServices] IAlterarSenhaUseCase useCase,
            [FromBody] RequisicaoAlterarSenhaJson request)
        {
            await useCase.Executar(request);

            return NoContent();
        }
    }
}