using CORE.Api.Helpers;
using CORE.Api.Responses;
using CORE.Application.Interfaces;
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
    private readonly CombaterCivilizacoes combaterCivilizacoes;
    private readonly IPartidaRepository partidaRepository;

    public PartidaController(
        CriarPartida criarPartida,
        AvancarTurnoPartida avancarTurnoPartida,
        ILogger<PartidaController> logger,
        CombaterCivilizacoes combaterCivilizacoes, IPartidaRepository partidaRepository)
    {
        this.criarPartida = criarPartida;
        this.avancarTurnoPartida = avancarTurnoPartida;
        this.logger = logger;
        this.combaterCivilizacoes = combaterCivilizacoes;
        this.partidaRepository = partidaRepository;
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


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var partida = await partidaRepository.GetByIdComCivilizacoesAsync(id);

        if (partida is null)
            return NotFound("Partida não encontrada.");

        return Ok(MapearResponse(partida));
    }



    [HttpPost("{id:guid}/combate")]
    public async Task<IActionResult> CombateAsync(
        Guid id,
        [FromQuery] Guid atacanteId,
        [FromQuery] Guid defensorId)
    {
        logger.LogInformation("Requisição recebida: combate na partida {PartidaId}.", id);
        try
        {
            var resultado = await combaterCivilizacoes.ExecutarAsync(id, atacanteId, defensorId);
            return Ok(new CombateResponse(
                resultado.Descricao,
                resultado.Vencedor.Nome,
                resultado.Perdedor.Nome,
                resultado.DiferencaPoderMilitar,
                resultado.RecursosRoubados,
                resultado.TerritoriosRoubados,
                MapearCivilizacao(resultado.Atacante),
                MapearCivilizacao(resultado.Defensor)
            ));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro no combate da partida {PartidaId}.", id);
            return BadRequest(ex.Message);
        }
    }

    private PartidaResponse MapearResponse(CORE.Domain.Entities.Partida partida)
    {
        var civs = partida.Civilizacoes.Select(c => MapearCivilizacao(c)).ToList();

        return new PartidaResponse(
            partida.Id,
            partida.Nome,
            partida.TurnoAtual,
            partida.Encerrada,
            civs
        );
    }

    private CivilizacaoResponse MapearCivilizacao(CORE.Domain.Entities.Civilizacao civilizacao)
    {
        return new CivilizacaoResponse(
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
            EventoHelper.Traduzir(civilizacao.UltimoEvento)
        );
    }
}