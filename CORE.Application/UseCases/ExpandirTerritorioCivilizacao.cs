using CORE.Application.Interfaces;
using CORE.Domain.Entities;
using CORE.Domain.Services;
using Microsoft.Extensions.Logging;

namespace CORE.Application.UseCases;

public class ExpandirTerritorioCivilizacao
{
    private readonly ICivilizacaoRepository civilizacaoRepository;
    private readonly IRegiaoRepository regiaoRepository;
    private readonly ILogger<ExpandirTerritorioCivilizacao> logger;

    public ExpandirTerritorioCivilizacao(
        ICivilizacaoRepository civilizacaoRepository,
        IRegiaoRepository regiaoRepository,
        ILogger<ExpandirTerritorioCivilizacao> logger)
    {
        this.civilizacaoRepository = civilizacaoRepository;
        this.regiaoRepository = regiaoRepository;
        this.logger = logger;
    }

    public async Task<Civilizacao> ExecutarAsync(Guid civilizacaoId)
    {
        var civilizacao = await civilizacaoRepository.GetByIdAsync(civilizacaoId);
        if (civilizacao is null)
            throw new Exception("Civilização não encontrada.");

        // busca todas as regiões da partida pra não sobrepor
        var todasRegioes = civilizacao.PartidaId.HasValue
           ? await regiaoRepository.GetByPartidaAsync(civilizacao.PartidaId.Value)
            : await regiaoRepository.GetControladasAsync(civilizacaoId);

        var posicao = MapaHelper.EncontrarCelulaAdjacente(todasRegioes, civilizacaoId);

        if (posicao.HasValue)
        {
            var terreno = MapaHelper.TerremoAleatorio();
            var novaRegiao = new Regiao(
                $"Território de {civilizacao.Nome}",
                terreno,
                civilizacaoId,
                posicao.Value.x,
                posicao.Value.y);
            novaRegiao.MarcarComoControlada();

            await regiaoRepository.AddAsync(novaRegiao);
            logger.LogInformation("{Civ} expandiu para ({X},{Y}) — {Terreno}",
                civilizacao.Nome, posicao.Value.x, posicao.Value.y, terreno);
        }

        civilizacao.AdicionarTerritorio();
        await civilizacaoRepository.UpdateAsync(civilizacao);
        return civilizacao;
    }
}