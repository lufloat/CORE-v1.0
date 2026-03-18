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
    private readonly AvancarTurnoCivilizacao avancarTurnoCivilizacao;

    public CivilizacaoController(
        IniciarCivilizacao iniciarCivilizacao,
        AvancarTurnoCivilizacao avancarTurnoCivilizacao)
    {
        this.iniciarCivilizacao = iniciarCivilizacao;
        this.avancarTurnoCivilizacao = avancarTurnoCivilizacao;
        this.expandirTerritorio = expandirTerritorio;
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
            civilizacao.Era.ToString(),
            civilizacao.UltimoEvento);

        return Ok(response);
    }

    [HttpPost("{id:guid}/expandir")]
    public async Task<IActionResult> ExpandirAsync(Guid id)
    {
        var civilizacao = await expandirTerritorio.ExecutarAsync(id);

        return Ok(new CivilizacaoResponse(
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
            civilizacao.Era.ToString(),
            civilizacao.UltimoEvento));
    }

    [HttpPost("{id:guid}/avancar-turno")]
    public async Task<IActionResult> AvancarTurnoAsync(Guid id)
    {
        try
        {
            var civilizacao = await avancarTurnoCivilizacao.ExecutarAsync(id);

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
                civilizacao.Era.ToString(),
                civilizacao.UltimoEvento);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}