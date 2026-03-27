using CORE.Application.Interfaces;
using CORE.Domain.Entities;
using CORE.Domain.Services;
using Microsoft.Extensions.Logging;

namespace CORE.Application.UseCases;

public class AvancarTurnoPartida
{
    private readonly IPartidaRepository partidaRepository;
    private readonly IRegiaoRepository regiaoRepository;
    private readonly IIAService iaService;
    private readonly ILogger<AvancarTurnoPartida> logger;

    public AvancarTurnoPartida(
        IPartidaRepository partidaRepository,
        IRegiaoRepository regiaoRepository,
        IIAService iaService,
        ILogger<AvancarTurnoPartida> logger)
    {
        this.partidaRepository = partidaRepository;
        this.regiaoRepository = regiaoRepository;
        this.iaService = iaService;
        this.logger = logger;
    }

    public async Task<Partida> ExecutarAsync(Guid partidaId)
    {
        logger.LogInformation("Iniciando turno da partida {PartidaId}", partidaId);

        var partida = await partidaRepository.GetByIdComCivilizacoesAsync(partidaId);

        if (partida is null)
        {
            logger.LogError("Partida não encontrada: {PartidaId}", partidaId);
            throw new Exception("Partida não encontrada.");
        }

        if (partida.Encerrada)
        {
            logger.LogWarning("Tentativa de avançar turno em partida encerrada: {PartidaId}", partidaId);
            throw new Exception("Esta partida já foi encerrada.");
        }

        foreach (var civilizacao in partida.Civilizacoes)
        {
            var regioes = await regiaoRepository.GetControladasAsync(civilizacao.Id);
            var decisao = await iaService.ObterDecisaoAsync(civilizacao);
            civilizacao.AplicarDecisao(decisao);
            civilizacao.AvancarTurno(regioes);

            // auto-expande quando atinge novos marcos populacionais
            if (civilizacao.Populacao > 0 && civilizacao.Populacao % 20 == 0)
            {
                var todasRegioes = civilizacao.PartidaId.HasValue
                    ? await regiaoRepository.GetByPartidaAsync(civilizacao.PartidaId.Value)
                    : regioes;

                var posicao = MapaHelper.EncontrarCelulaAdjacente(todasRegioes, civilizacao.Id);
                if (posicao.HasValue)
                {
                    var terreno = MapaHelper.TerremoAleatorio();
                    var novaRegiao = new Regiao(
                        $"Território de {civilizacao.Nome}",
                        terreno,
                        civilizacao.Id,
                        posicao.Value.x,
                        posicao.Value.y);
                    novaRegiao.MarcarComoControlada();
                    await regiaoRepository.AddAsync(novaRegiao);
                    civilizacao.AdicionarTerritorio();
                    logger.LogInformation("{Civ} auto-expandiu para ({X},{Y})",
                        civilizacao.Nome, posicao.Value.x, posicao.Value.y);
                }
            }
        }

        partida.AvancarTurno();
        logger.LogInformation("Turno {Turno} da partida {PartidaId} concluído.",
            partida.TurnoAtual, partidaId);

        await partidaRepository.UpdateAsync(partida);
        return partida;
    }
}