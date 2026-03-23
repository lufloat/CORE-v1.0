using CORE.Api.Helpers;
using CORE.Api.Requests;
using CORE.Api.Responses;
using CORE.Application.Interfaces;
using CORE.Application.UseCases;
using CORE.Domain.Entities;
using CORE.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CORE.Api.Controllers;

[ApiController]
[Route("api/civilizacao")]
public class CivilizacaoController : ControllerBase
{
    private readonly IniciarCivilizacao iniciarCivilizacao;
    private readonly AvancarTurnoCivilizacao avancarTurnoCivilizacao;
    private readonly ExpandirTerritorioCivilizacao expandirTerritorio;
    private readonly ICivilizacaoRepository civilizacaoRepository;
    private readonly AplicarDecisaoCivilizacao aplicarDecisao;
    private readonly SimularCivilizacao simularCivilizacao;
    private readonly ILogger<CivilizacaoController> logger;

    public CivilizacaoController(
        IniciarCivilizacao iniciarCivilizacao,
        AvancarTurnoCivilizacao avancarTurnoCivilizacao,
        ExpandirTerritorioCivilizacao expandirTerritorio,
        ICivilizacaoRepository civilizacaoRepository,
        AplicarDecisaoCivilizacao aplicarDecisao,
        SimularCivilizacao simularCivilizacao,
        ILogger<CivilizacaoController> logger)
    {
        this.iniciarCivilizacao = iniciarCivilizacao;
        this.avancarTurnoCivilizacao = avancarTurnoCivilizacao;
        this.expandirTerritorio = expandirTerritorio;
        this.civilizacaoRepository = civilizacaoRepository;
        this.aplicarDecisao = aplicarDecisao;
        this.simularCivilizacao = simularCivilizacao;
        this.logger = logger;
    }

    [HttpPost("iniciar")]
    public async Task<IActionResult> IniciarAsync(IniciarCivilizacaoRequest request)
    {
        logger.LogInformation("Requisição recebida: iniciar civilização '{Nome}'.", request.Nome);

        if (string.IsNullOrWhiteSpace(request.Nome))
        {
            logger.LogWarning("Tentativa de iniciar civilização sem nome.");
            return BadRequest("O nome da civilização é obrigatório.");
        }

        var civilizacao = await iniciarCivilizacao.ExecutarAsync(request.Nome);
        logger.LogInformation("Civilização '{Nome}' criada com id {Id}.", civilizacao.Nome, civilizacao.Id);

        return Ok(MapearResponse(civilizacao));
    }

    [HttpPost("{id:guid}/expandir")]
    public async Task<IActionResult> ExpandirAsync(Guid id)
    {
        logger.LogInformation("Requisição recebida: expandir território da civilização {Id}.", id);
        var civilizacao = await expandirTerritorio.ExecutarAsync(id);
        logger.LogInformation("Civilização {Id} expandiu para {Territorios} territórios.", id, civilizacao.Territorios);
        return Ok(MapearResponse(civilizacao));
    }

    [HttpPost("{id:guid}/avancar-turno")]
    public async Task<IActionResult> AvancarTurnoAsync(Guid id)
    {
        logger.LogInformation("Requisição recebida: avançar turno da civilização {Id}.", id);
        try
        {
            var civilizacao = await avancarTurnoCivilizacao.ExecutarAsync(id);
            logger.LogInformation("Civilização {Id} avançou para o turno {Turno}.", id, civilizacao.Turno);
            return Ok(MapearResponse(civilizacao));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao avançar turno da civilização {Id}.", id);
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{id:guid}/decisao")]
    public async Task<IActionResult> AplicarDecisaoAsync(Guid id, [FromQuery] string decisao)
    {
        logger.LogInformation("Requisição recebida: aplicar decisão '{Decisao}' na civilização {Id}.", decisao, id);

        if (!Enum.TryParse<TipoDecisao>(decisao, true, out var tipo))
        {
            logger.LogWarning("Decisão inválida recebida: '{Decisao}'.", decisao);
            return BadRequest("Decisão inválida.");
        }

        var civilizacao = await aplicarDecisao.ExecutarAsync(id, tipo);
        logger.LogInformation("Decisão '{Decisao}' aplicada na civilização {Id}.", decisao, id);
        return Ok(MapearResponse(civilizacao));
    }

    [HttpPost("{id:guid}/simular")]
    public async Task<IActionResult> SimularAsync(Guid id, [FromQuery] int turnos = 10)
    {
        logger.LogInformation("Requisição recebida: simular {Turnos} turnos da civilização {Id}.", turnos, id);

        var (civilizacao, historico) = await simularCivilizacao.ExecutarAsync(id, turnos);

        logger.LogInformation("Simulação concluída. Civilização {Id} no turno {Turno}.", id, civilizacao.Turno);

        var histResponse = historico.Select(h => new SimulacaoTurnoResponse(
            h.Turno, h.Populacao, h.Comida, h.Madeira, h.Pedra,
            h.Moral, h.Tecnologia, h.PoderMilitar, h.Territorios,
            h.Era, h.Decisao, EventoHelper.Traduzir(Enum.Parse<TipoEvento>(h.Evento))
        )).ToList();

        return Ok(new SimulacaoResponse(turnos, histResponse, MapearResponse(civilizacao)));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        logger.LogInformation("Requisição recebida: buscar civilização {Id}.", id);
        var civilizacao = await civilizacaoRepository.GetByIdAsync(id);

        if (civilizacao is null)
        {
            logger.LogWarning("Civilização não encontrada: {Id}.", id);
            return NotFound("Civilização não encontrada.");
        }

        return Ok(MapearResponse(civilizacao));
    }

    private CivilizacaoResponse MapearResponse(Civilizacao civilizacao) => new(
        civilizacao.Id, civilizacao.Nome, civilizacao.Turno,
        civilizacao.Populacao, civilizacao.Comida, civilizacao.Madeira,
        civilizacao.Pedra, civilizacao.Moral, civilizacao.Tecnologia,
        civilizacao.PoderMilitar, civilizacao.Territorios,
        civilizacao.Era.ToString(),
        EventoHelper.Traduzir(civilizacao.UltimoEvento));
}