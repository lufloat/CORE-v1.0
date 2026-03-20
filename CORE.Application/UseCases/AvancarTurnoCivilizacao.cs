using CORE.Application.Interfaces;
using CORE.Domain.Entities;

namespace CORE.Application.UseCases;

public class AvancarTurnoCivilizacao
{
    private readonly ICivilizacaoRepository civilizacaoRepository;
    private readonly IRegiaoRepository regiaoRepository;
    private readonly IIAService iaservice;

    public AvancarTurnoCivilizacao(
        ICivilizacaoRepository civilizacaoRepository,
        IRegiaoRepository regiaoRepository,
        IIAService iaservice)  // ← ADICIONA AQUI
    {
        this.civilizacaoRepository = civilizacaoRepository;
        this.regiaoRepository = regiaoRepository;
        this.iaservice = iaservice;  // ← E AQUI
    }

    public async Task<Civilizacao> ExecutarAsync(Guid civilizacaoId)
    {
        var civilizacao = await civilizacaoRepository.GetByIdAsync(civilizacaoId);
        if (civilizacao is null)
            throw new Exception("Civilização não encontrada.");

        var regioesControladas = await regiaoRepository.GetControladasAsync(civilizacaoId);

        var decisao = await iaservice.ObterDecisaoAsync(civilizacao);

        civilizacao.AplicarDecisao(decisao);

        civilizacao.AvancarTurno(regioesControladas);
        await civilizacaoRepository.UpdateAsync(civilizacao);
        return civilizacao;
    }
}