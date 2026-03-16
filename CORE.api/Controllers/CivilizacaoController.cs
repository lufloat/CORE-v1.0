using CORE.Api.Requests;
using CORE.Api.Responses;
using CORE.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CORE.Api.Controllers;

[ApiController]
[Route("api/civilizacao")]
public class CivilizacaoController : ControllerBase
{
    private readonly IniciarCivilizacao iniciarCivilizacao;

    public CivilizacaoController(IniciarCivilizacao iniciarCivilizacao)
    {
        this.iniciarCivilizacao = iniciarCivilizacao;
    }

    [HttpPost("iniciar")]
    public async Task<IActionResult> IniciarAsync(IniciarCivilizacaoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
            return BadRequest("O nome da civilização é obrigatório.");

        var civilizacao = await iniciarCivilizacao.ExecutarAsync(request.Nome);

        var response = new CivilizacaoResponse(
            civilizacao.Id,
            civilizacao.Nome,
            civilizacao.Turno,
            civilizacao.Populacao,
            civilizacao.Comida,
            civilizacao.Madeira,
            civilizacao.Pedra,
            civilizacao.Moral,
            civilizacao.Tecnologia,
            civilizacao.PoderMilitar,
            civilizacao.Territorios,
            civilizacao.Era.ToString());

        return Ok(response);
    }
}