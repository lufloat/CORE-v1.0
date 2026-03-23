using CORE.Application.Interfaces;
using CORE.Domain.Entities;
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

            logger.LogInformation("[Turno {Turno}] {Civilizacao} decidiu: {Decisao}",
                partida.TurnoAtual + 1, civilizacao.Nome, decisao);

            // alertas de recursos críticos
            if (civilizacao.Comida < 15)
                logger.LogWarning("{Civilizacao} está com comida crítica: {Comida}",
                    civilizacao.Nome, civilizacao.Comida);

            if (civilizacao.Moral < 20)
                logger.LogWarning("{Civilizacao} está com moral crítica: {Moral}",
                    civilizacao.Nome, civilizacao.Moral);

            if (civilizacao.Populacao <= 0)
                logger.LogError("{Civilizacao} ficou sem população! Turno {Turno}",
                    civilizacao.Nome, partida.TurnoAtual + 1);

            civilizacao.AplicarDecisao(decisao);
            civilizacao.AvancarTurno(regioes);
        }

        partida.AvancarTurno();
        logger.LogInformation("Turno {Turno} da partida {PartidaId} concluído.",
            partida.TurnoAtual, partidaId);

        await partidaRepository.UpdateAsync(partida);
        return partida;
    }
}