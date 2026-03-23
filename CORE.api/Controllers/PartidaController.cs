using CORE.Api.Helpers;
using CORE.Api.Responses;
using CORE.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CORE.Api.Controllers;

[ApiController]
[Route("api/partida")]
public class PartidaController : ControllerBase
{
    private readonly CriarPartida criarPartida;
    private readonly AvancarTurnoPartida avancarTurnoPartida;
    private readonly ILogger<PartidaController> logger;

    public PartidaController(
        CriarPartida criarPartida,
        AvancarTurnoPartida avancarTurnoPartida,
        ILogger<PartidaController> logger)
    {
        this.criarPartida = criarPartida;
        this.avancarTurnoPartida = avancarTurnoPartida;
        this.logger = logger;
    }

    [HttpPost("criar")]
    public async Task<IActionResult> CriarAsync([FromBody] List<string> nomes)
    {
        logger.LogInformation("Requisição recebida: criar partida com {Total} civilizações.", nomes?.Count ?? 0);

        if (nomes is null || nomes.Count < 2)
        {
            logger.LogWarning("Requisição inválida: menos de 2 civilizações informadas.");
            return BadRequest("Informe pelo menos 2 nomes de civilizações.");
        }

        var partida = await criarPartida.ExecutarAsync(nomes);

        logger.LogInformation("Partida {PartidaId} criada com sucesso.", partida.Id);
        return Ok(MapearResponse(partida));
    }

    [HttpPost("{id:guid}/avancar-turno")]
    public async Task<IActionResult> AvancarTurnoAsync(Guid id)
    {
        logger.LogInformation("Requisição recebida: avançar turno da partida {PartidaId}.", id);

        try
        {
            var partida = await avancarTurnoPartida.ExecutarAsync(id);
            logger.LogInformation("Turno avançado com sucesso. Partida {PartidaId} no turno {Turno}.",
                id, partida.TurnoAtual);
            return Ok(MapearResponse(partida));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao avançar turno da partida {PartidaId}.", id);
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