using CORE.Application.Interfaces;
using CORE.Domain.Entities;

namespace CORE.Application.UseCases;

public class AvancarTurnoCivilizacao
{
    private readonly ICivilizacaoRepository civilizacaoRepository;
    private readonly IRegiaoRepository regiaoRepository;

    public AvancarTurnoCivilizacao(
        ICivilizacaoRepository civilizacaoRepository,
        IRegiaoRepository regiaoRepository)
    {
        this.civilizacaoRepository = civilizacaoRepository;
        this.regiaoRepository = regiaoRepository;
    }

    public async Task<Civilizacao> ExecutarAsync(Guid civilizacaoId)
    {
        var civilizacao = await civilizacaoRepository.GetByIdAsync(civilizacaoId);

        if (civilizacao is null)
            throw new Exception("Civilização não encontrada.");

        var regioesControladas = await regiaoRepository.GetControladasAsync();

        civilizacao.AvancarTurno(regioesControladas);

        await civilizacaoRepository.UpdateAsync(civilizacao);

        return civilizacao;
    }
}