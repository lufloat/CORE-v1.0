using CORE.Application.Interfaces;
using CORE.Domain.Entities;

namespace CORE.Application.UseCases;

public class AvancarTurnoPartida
{
    private readonly IPartidaRepository partidaRepository;
    private readonly IRegiaoRepository regiaoRepository;
    private readonly IIAService iaService;

    public AvancarTurnoPartida(
        IPartidaRepository partidaRepository,
        IRegiaoRepository regiaoRepository,
        IIAService iaService)
    {
        this.partidaRepository = partidaRepository;
        this.regiaoRepository = regiaoRepository;
        this.iaService = iaService;
    }

    public async Task<Partida> ExecutarAsync(Guid partidaId)
    {
        var partida = await partidaRepository.GetByIdComCivilizacoesAsync(partidaId);

        if (partida is null)
            throw new Exception("Partida não encontrada.");

        if (partida.Encerrada)
            throw new Exception("Esta partida já foi encerrada.");

        foreach (var civilizacao in partida.Civilizacoes)
        {
            var regioes = await regiaoRepository.GetControladasAsync(civilizacao.Id);
            var decisao = await iaService.ObterDecisaoAsync(civilizacao);
            civilizacao.AplicarDecisao(decisao);
            civilizacao.AvancarTurno(regioes);
        }

        partida.AvancarTurno();

        await partidaRepository.UpdateAsync(partida);
        return partida;
    }
}