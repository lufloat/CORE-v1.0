using CORE.Api.Helpers;
using CORE.Api.Requests;
using CORE.Api.Responses;
using CORE.Application.Interfaces;
using CORE.Application.UseCases;
using CORE.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CORE.Api.Controllers;

[ApiController]
[Route("api/civilizacao")]
public class CivilizacaoController : ControllerBase
{
    private readonly IniciarCivilizacao iniciarCivilizacao;
    private readonly AvancarTurnoCivilizacao avancarTurnoCivilizacao;
    private readonly ExpandirTerritorioCivilizacao expandirTerritorio;
    private readonly ICivilizacaoRepository civilizacaoRepository; // ✅ declarado
    private readonly AplicarDecisaoCivilizacao aplicarDecisao;


    public CivilizacaoController(
        IniciarCivilizacao iniciarCivilizacao,
        AvancarTurnoCivilizacao avancarTurnoCivilizacao,
        ExpandirTerritorioCivilizacao expandirTerritorio,
        ICivilizacaoRepository civilizacaoRepository,
         AplicarDecisaoCivilizacao aplicarDecisao) // ✅ injetado
    {
        this.iniciarCivilizacao = iniciarCivilizacao;
        this.avancarTurnoCivilizacao = avancarTurnoCivilizacao;
        this.expandirTerritorio = expandirTerritorio;
        this.civilizacaoRepository = civilizacaoRepository;
        this.aplicarDecisao = aplicarDecisao;// ✅ atribuído
    }

    [HttpPost("iniciar")]
    public async Task<IActionResult> IniciarAsync(IniciarCivilizacaoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
            return BadRequest("O nome da civilização é obrigatório.");

        var civilizacao = await iniciarCivilizacao.ExecutarAsync(request.Nome);

        return Ok(new CivilizacaoResponse(
            civilizacao.Id, civilizacao.Nome, civilizacao.Turno,
            civilizacao.Populacao, civilizacao.Comida, civilizacao.Madeira,
            civilizacao.Pedra, civilizacao.Moral, civilizacao.Tecnologia,
            civilizacao.PoderMilitar, civilizacao.Territorios,
            civilizacao.Era.ToString(),
            EventoHelper.Traduzir(civilizacao.UltimoEvento)));
    }

    [HttpPost("{id:guid}/expandir")]
    public async Task<IActionResult> ExpandirAsync(Guid id)
    {
        var civilizacao = await expandirTerritorio.ExecutarAsync(id);

        return Ok(new CivilizacaoResponse(
            civilizacao.Id, civilizacao.Nome, civilizacao.Turno,
            civilizacao.Populacao, civilizacao.Comida, civilizacao.Madeira,
            civilizacao.Pedra, civilizacao.Moral, civilizacao.Tecnologia,
            civilizacao.PoderMilitar, civilizacao.Territorios,
            civilizacao.Era.ToString(),
            EventoHelper.Traduzir(civilizacao.UltimoEvento)));
    }

    [HttpPost("{id:guid}/avancar-turno")]
    public async Task<IActionResult> AvancarTurnoAsync(Guid id)
    {
        try
        {
            var civilizacao = await avancarTurnoCivilizacao.ExecutarAsync(id);

            return Ok(new CivilizacaoResponse(
                civilizacao.Id, civilizacao.Nome, civilizacao.Turno,
                civilizacao.Populacao, civilizacao.Comida, civilizacao.Madeira,
                civilizacao.Pedra, civilizacao.Moral, civilizacao.Tecnologia,
                civilizacao.PoderMilitar, civilizacao.Territorios,
                civilizacao.Era.ToString(),
                EventoHelper.Traduzir(civilizacao.UltimoEvento)));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    } // ✅ AvancarTurnoAsync fechado corretamente aqui

    [HttpPost("{id:guid}/decisao")]
    public async Task<IActionResult> AplicarDecisaoAsync(Guid id, [FromQuery] string decisao)
    {
        if (!Enum.TryParse<TipoDecisao>(decisao, true, out var tipo))
            return BadRequest("Decisão inválida.");

        var civilizacao = await aplicarDecisao.ExecutarAsync(id, tipo);

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
            EventoHelper.Traduzir(civilizacao.UltimoEvento)
        ));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var civilizacao = await civilizacaoRepository.GetByIdAsync(id);

        if (civilizacao is null)
            return NotFound("Civilização não encontrada.");

        return Ok(new CivilizacaoResponse(
            civilizacao.Id, civilizacao.Nome, civilizacao.Turno,
            civilizacao.Populacao, civilizacao.Comida, civilizacao.Madeira,
            civilizacao.Pedra, civilizacao.Moral, civilizacao.Tecnologia,
            civilizacao.PoderMilitar, civilizacao.Territorios,
            civilizacao.Era.ToString(),
            EventoHelper.Traduzir(civilizacao.UltimoEvento)));
    }
}