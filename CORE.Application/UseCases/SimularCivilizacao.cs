using CORE.Application.Interfaces;
using CORE.Domain.Entities;

namespace CORE.Application.UseCases;

public class SimularCivilizacao
{
    private readonly ICivilizacaoRepository civilizacaoRepository;
    private readonly IRegiaoRepository regiaoRepository;
    private readonly IIAService iaService;

    public SimularCivilizacao(
        ICivilizacaoRepository civilizacaoRepository,
        IRegiaoRepository regiaoRepository,
        IIAService iaService)
    {
        this.civilizacaoRepository = civilizacaoRepository;
        this.regiaoRepository = regiaoRepository;
        this.iaService = iaService;
    }

    public async Task<(Civilizacao civilizacao, List<SnapshotTurno> historico)> ExecutarAsync(Guid id, int turnos)
    {
        var civilizacao = await civilizacaoRepository.GetByIdAsync(id);
        if (civilizacao is null)
            throw new Exception("Civilização não encontrada.");

        var historico = new List<SnapshotTurno>();

        for (int i = 0; i < turnos; i++)
        {
            var regioes = await regiaoRepository.GetControladasAsync(id);
            var decisao = await iaService.ObterDecisaoAsync(civilizacao);
            civilizacao.AplicarDecisao(decisao);
            civilizacao.AvancarTurno(regioes);

            historico.Add(new SnapshotTurno(
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
                decisao.ToString(),
                civilizacao.UltimoEvento.ToString()
            ));
        }

        await civilizacaoRepository.UpdateAsync(civilizacao);
        return (civilizacao, historico);
    }
}

public record SnapshotTurno(
    int Turno,
    int Populacao,
    int Comida,
    int Madeira,
    int Pedra,
    int Moral,
    int Tecnologia,
    int PoderMilitar,
    int Territorios,
    string Era,
    string Decisao,
    string Evento
);