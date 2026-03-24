using CORE.Api.Responses;
using CORE.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CORE.Api.Controllers;

[ApiController]
[Route("api/regiao")]
public class RegiaoController : ControllerBase
{
    private readonly IRegiaoRepository regiaoRepository;
    private readonly ICivilizacaoRepository civilizacaoRepository;

    public RegiaoController(
        IRegiaoRepository regiaoRepository,
        ICivilizacaoRepository civilizacaoRepository)
    {
        this.regiaoRepository = regiaoRepository;
        this.civilizacaoRepository = civilizacaoRepository;
    }

    [HttpGet("partida/{partidaId:guid}")]
    public async Task<IActionResult> GetByPartidaAsync(Guid partidaId)
    {
        var civilizacoes = await civilizacaoRepository.GetByPartidaIdAsync(partidaId);

        var regioes = new List<RegiaoResponse>();

        foreach (var civ in civilizacoes)
        {
            var regioesDaCiv = await regiaoRepository.GetControladasAsync(civ.Id);
            foreach (var regiao in regioesDaCiv)
            {
                regioes.Add(new RegiaoResponse(
                    regiao.Id,
                    regiao.Nome,
                    regiao.Terreno.ToString(),
                    regiao.Controlada,
                    regiao.X,
                    regiao.Y,
                    civ.Id,
                    civ.Nome
                ));
            }
        }

        return Ok(regioes);
    }
}