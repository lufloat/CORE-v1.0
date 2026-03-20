using CORE.Api.Helpers;
using CORE.Api.Responses;
using CORE.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CORE.Api.Controllers;

[ApiController]
[Route("api/partida")]
public class PartidaController : ControllerBase
{
    private readonly CriarPartida criarPartida;
    private readonly AvancarTurnoPartida avancarTurnoPartida;

    public PartidaController(
        CriarPartida criarPartida,
        AvancarTurnoPartida avancarTurnoPartida)
    {
        this.criarPartida = criarPartida;
        this.avancarTurnoPartida = avancarTurnoPartida;
    }

    [HttpPost("criar")]
    public async Task<IActionResult> CriarAsync([FromBody] List<string> nomes)
    {
        if (nomes is null || nomes.Count < 2)
            return BadRequest("Informe pelo menos 2 nomes de civilizações.");

        var partida = await criarPartida.ExecutarAsync(nomes);

        return Ok(MapearResponse(partida));
    }

    [HttpPost("{id:guid}/avancar-turno")]
    public async Task<IActionResult> AvancarTurnoAsync(Guid id)
    {
        try
        {
            var partida = await avancarTurnoPartida.ExecutarAsync(id);
            return Ok(MapearResponse(partida));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private PartidaResponse MapearResponse(CORE.Domain.Entities.Partida partida)
    {
        var civs = partida.Civilizacoes.Select(c => new CivilizacaoResponse(
            c.Id, c.Nome, c.Turno,
            c.Populacao, c.Comida, c.Madeira,
            c.Pedra, c.Moral, c.Tecnologia,
            c.PoderMilitar, c.Territorios,
            c.Era.ToString(),
            EventoHelper.Traduzir(c.UltimoEvento)
        )).ToList();

        return new PartidaResponse(
            partida.Id,
            partida.Nome,
            partida.TurnoAtual,
            partida.Encerrada,
            civs
        );
    }
}